using Microsoft.Extensions.Configuration;
using NovaEcommerce.Domain.Entities;
using NovaEcommerce.ServicesApp.DTOs.Responses;
using NovaEcommerce.ServicesApp.DTOs.WishlistDtos;
using NovaEcommerce.ServicesApp.Services.Interfaces.Repository;
using NovaEcommerce.ServicesApp.Services.Interfaces.Service;


namespace NovaEcommerce.ServicesApp.Services.Implementations
{
    public class WishlistService(IWishlistRepository repository, IConfiguration configuration) : IWishlistService
    {
        public async Task<ApiResponse<WishlistItemDto>> AddItem(int userId, int wishlistId, AddWishlistItemRequestDto request)
        {
            var wishlist = await repository.GetWishlistsById(userId, wishlistId);
            if (wishlist is null) return ApiResponse<WishlistItemDto>.FailResponse("wishlist not found", 404);

            var variant = await repository.GetProductVariant(request.ProductVariantId);
            if (variant is null) return ApiResponse<WishlistItemDto>.FailResponse("Product variant not found", 404);

            //elave edilen itemin wishlistde olub olmamasinin yoxlanmasi
            var existing = await repository.GetWishlistItemByVariant(wishlistId, request.ProductVariantId);
            if (existing is not null) return ApiResponse<WishlistItemDto>.FailResponse("Item already exists in wishlist", 409);

            var item = new WishlistItem
            {
                WishlistId = wishlistId,
                ProductVariantId = request.ProductVariantId,
                PriceAtAdd = variant.Price,
                AddedAt = DateTime.UtcNow
            };

            repository.AddWishlistItem(item);
            await repository.SaveChangesAsync();

            item.ProductVariant = variant;
            return ApiResponse<WishlistItemDto>.SuccessResponse(MapToDto(item));
        }

        public async Task<ApiResponse<WishlistDto>> CreateWishlist(int userId, CreateWishlistRequestDto request)
        {
            if (string.IsNullOrWhiteSpace(request.Name)) return ApiResponse<WishlistDto>.FailResponse("Name is required", 400);

            var newWishlist = new Wishlist
            {
                UserId = userId,
                Name = request.Name,
            };

            repository.AddWishlist(newWishlist);
            await repository.SaveChangesAsync();

            return ApiResponse<WishlistDto>.SuccessResponse(new WishlistDto { Id = newWishlist.Id, ItemCount = 0, Name = newWishlist.Name }, "Wishlist created", 201);

        }

        public async Task<ApiResponse<IReadOnlyCollection<WishlistItemDto>>> GetWishlistItems(int userId, int wishlistId)
        {
            var wishlist = await repository.GetWishlistsById(userId, wishlistId);
            if (wishlist is null) return ApiResponse<IReadOnlyCollection<WishlistItemDto>>.FailResponse("wishlist not found", 404);

            var dto = wishlist.Items.Select(MapToDto).ToList();
            return ApiResponse<IReadOnlyCollection<WishlistItemDto>>.SuccessResponse(dto);
        }

        public async Task<ApiResponse<IReadOnlyCollection<WishlistDto>>> GetWishlists(int userId)
        {
            var wishlists = await repository.GetWishlists(userId);

            var dto = wishlists.Select(w => new WishlistDto
            {
                Id = w.Id,
                Name = w.Name,
                ItemCount = w.Items.Count
            }).ToList();

            return ApiResponse<IReadOnlyCollection<WishlistDto>>.SuccessResponse(dto);
        }

        public async Task<ApiResponse<List<WishlistItemDto>>> RemoveWishlistItem(int userId, int wishlistId, int itemId)
        {
            var wishlist = await repository.GetWishlistsById(userId, wishlistId);
            if (wishlist is null) return ApiResponse<List<WishlistItemDto>>.FailResponse("Wishlist doesnt exist", 404);

            var item = await repository.GetWishlistItem(wishlistId, itemId);
            if (item is null) return ApiResponse<List<WishlistItemDto>>.FailResponse("Item doesnt exist", 404);

            repository.RemoveWishlistItem(item);
            await repository.SaveChangesAsync();

            var updatedWishlist = await repository.GetWishlistsById(userId, wishlistId);
            var dto = updatedWishlist!.Items.Select(MapToDto).ToList();
            return ApiResponse<List<WishlistItemDto>>.SuccessResponse(dto, "removed from wishlist", 204);
        }

        public async Task<ApiResponse<bool>> RequestNotify(int userId, int wishlistId, int itemId)
        {
            var wishlist = await repository.GetWishlistsById(userId, wishlistId);
            if (wishlist is null) return ApiResponse<bool>.FailResponse("wishlist doesnt exist", 404);

            var item = await repository.GetWishlistItem(wishlistId, itemId);
            if (item is null) return ApiResponse<bool>.FailResponse("item doesnt exist", 404);

            if (item.ProductVariant.StockQuantity > 0) return ApiResponse<bool>.FailResponse("item is in stock", 400);

            item.NotifyRequested = true;
            await repository.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "you will be notified.");
        }

        public async Task<ApiResponse<WishlistShareUrlDto>> ShareWishlistUrl(int userId, int wishlistId)
        {
            var wishlistById = await repository.GetWishlistsById(userId, wishlistId);

            if (wishlistById is null)
            {
                return ApiResponse<WishlistShareUrlDto>.FailResponse("wishlist is null", 404);
            }

            if (wishlistById.ShareToken is null) //eger token yoxdursa guid.tostring ile yenisin yaradirig ve repoda saveliyirik.
            {
                wishlistById.ShareToken = Guid.NewGuid().ToString("N");
                await repository.SaveChangesAsync();
            }

            var baseUrl = configuration.GetValue<string>("App:BaseUrl") ?? "siuuuuu.az"; //?? null olsa siuuu.az olsun url
            var url = $"{baseUrl.TrimEnd('/')}/wishlist/shared/{wishlistById.ShareToken}";//urlin linki 

            return ApiResponse<WishlistShareUrlDto>.SuccessResponse(new WishlistShareUrlDto { ShareUrl = url });
        }

        private static WishlistItemDto MapToDto(WishlistItem item)
        {
            var variant = item.ProductVariant;

            return new WishlistItemDto
            {
                Id = item.Id,
                ProductVariantId = variant.Id,
                ProductName = variant.Product.Name,
                Color = variant.Color,
                Size = variant.Size,
                CurrentPrice = variant.Price,
                PriceAtAdd = item.PriceAtAdd,
                PriceDropped = variant.Price < item.PriceAtAdd,
                OutOfStock = variant.StockQuantity <= 0,
                BackInStock = variant.StockQuantity > 0 && item.NotifyRequested,
                NotifyRequested = item.NotifyRequested,
                AddedAt = item.AddedAt
            };
        }
    }
}
