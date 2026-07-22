using Microsoft.Extensions.Configuration;
using NovaEcommerce.Domain.Entities;
using NovaEcommerce.Domain.Enums;
using NovaEcommerce.ServicesApp.DTOs.CartDtos;
using NovaEcommerce.ServicesApp.DTOs.Responses;
using NovaEcommerce.ServicesApp.Services.Interfaces.Repository;
using NovaEcommerce.ServicesApp.Services.Interfaces.Service;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.Services.Implementations
{
    public class CartService(ICartRepository repository, IConfiguration configuration) : ICartService
    {
        private const int LowStockThreshold = 5;

        public async Task<ApiResponse<CartDto>> GetCartAsync(int? userId, string? sessionId)
        {
            var cart = await repository.GetOrCreateCart(userId, sessionId);
            return ApiResponse<CartDto>.SuccessResponse(MapToDto(cart));
        }

        public async Task<ApiResponse<CartDto>> AddItemAsync(int? userId, string? sessionId, AddCartItemRequestDto request)
        {
            var cart = await repository.GetOrCreateCart(userId, sessionId);

            var variant = await repository.GetProductVariant(request.ProductVariantId);
            if (variant is null)
                return ApiResponse<CartDto>.FailResponse("Product variant not found", 404);

            var existingItem = await repository.GetCartItemByVariant(cart.Id, request.ProductVariantId);
            var requestedTotalQuantity = (existingItem?.Quantity ?? 0) + request.Quantity;

            if (requestedTotalQuantity <= 0) 
                return ApiResponse<CartDto>.FailResponse("Quantity must be greater than zero", 400);

            if (requestedTotalQuantity > variant.StockQuantity)
                return ApiResponse<CartDto>.FailResponse("Not enough stock available", 400);

            if (existingItem is not null)
            {
                existingItem.Quantity = requestedTotalQuantity;
            }
            else
            {
                repository.AddCartItem(new CartItem
                {
                    CartId = cart.Id,
                    ProductVariantId = request.ProductVariantId,
                    Quantity = request.Quantity,
                    IsSavedForLater = false,
                    AddedAt = DateTime.UtcNow
                });
            }

            await repository.SaveChangesAsync();

            var updatedCart = await repository.GetOrCreateCart(userId, sessionId);
            return ApiResponse<CartDto>.SuccessResponse(MapToDto(updatedCart), "Product added to cart");  
        }

        public async Task<ApiResponse<CartDto>> UpdateItemQuantityAsync(int? userId, string? sessionId, int cartItemId, int quantity)
        {
            var cart = await repository.GetOrCreateCart(userId, sessionId);

            var item = await repository.GetCartItem(cart.Id, cartItemId);
            if (item is null)
                return ApiResponse<CartDto>.FailResponse("Cart item not found", 404);

            if (item.ProductVariant.StockQuantity < quantity)
                return ApiResponse<CartDto>.FailResponse("Not enough stock available", 400);

            item.Quantity = quantity;
            await repository.SaveChangesAsync();

            var updatedCart = await repository.GetOrCreateCart(userId, sessionId);
            return ApiResponse<CartDto>.SuccessResponse(MapToDto(updatedCart), "Cart item quantity updated");
        }

        public async Task<ApiResponse<CartDto>> RemoveItemAsync(int? userId, string? sessionId, int cartItemId)
        {
            var cart = await repository.GetOrCreateCart(userId, sessionId);

            var item = await repository.GetCartItem(cart.Id, cartItemId);
            if (item is null)
                return ApiResponse<CartDto>.FailResponse("Cart item not found", 404);

            repository.RemoveCartItem(item);
            await repository.SaveChangesAsync();

            var updatedCart = await repository.GetOrCreateCart(userId, sessionId);
            return ApiResponse<CartDto>.SuccessResponse(MapToDto(updatedCart), "Cart item removed");
        }

        public async Task<ApiResponse<CartDto>> ToggleSaveForLaterAsync(int? userId, string? sessionId, int cartItemId)
        {
            var cart = await repository.GetOrCreateCart(userId, sessionId);

            var item = await repository.GetCartItem(cart.Id, cartItemId);
            if (item is null)
                return ApiResponse<CartDto>.FailResponse("Cart item not found", 404);

            item.IsSavedForLater = !item.IsSavedForLater;
            await repository.SaveChangesAsync();

            var updatedCart = await repository.GetOrCreateCart(userId, sessionId);
            return ApiResponse<CartDto>.SuccessResponse(MapToDto(updatedCart));
        }

        public async Task<ApiResponse<CartSummaryDto>> ApplyCouponAsync(int? userId, string? sessionId, ApplyCouponRequestDto request)
        {
            var cart = await repository.GetOrCreateCart(userId, sessionId);

            var coupon = await repository.GetActiveCouponByCode(request.Code);
            if (coupon is null)
                return ApiResponse<CartSummaryDto>.FailResponse("Coupon not found", 404);

            if (coupon.ExpiresAt is not null && coupon.ExpiresAt < DateTime.UtcNow)
                return ApiResponse<CartSummaryDto>.FailResponse("Coupon has expired", 400);

            var subtotal = cart.Items.Where(i => !i.IsSavedForLater).Sum(i => i.ProductVariant.Price * i.Quantity);

            if (coupon.MinOrderAmount is not null && subtotal < coupon.MinOrderAmount)
                return ApiResponse<CartSummaryDto>.FailResponse(
                    $"Minimum order amount is {coupon.MinOrderAmount}", 400);

            cart.AppliedCouponCode = coupon.Code;
            await repository.SaveChangesAsync();

            return ApiResponse<CartSummaryDto>.SuccessResponse(BuildSummary(cart, coupon, subtotal), "Coupon applied");
        }

        public async Task<ApiResponse<CartSummaryDto>> GetSummaryAsync(int? userId, string? sessionId)
        {
            var cart = await repository.GetOrCreateCart(userId, sessionId);

            var subtotal = cart.Items.Where(i => !i.IsSavedForLater).Sum(i => i.ProductVariant.Price * i.Quantity);

            Coupon? coupon = null;
            if (cart.AppliedCouponCode is not null)
            {
                coupon = await repository.GetActiveCouponByCode(cart.AppliedCouponCode);
            }

            return ApiResponse<CartSummaryDto>.SuccessResponse(BuildSummary(cart, coupon, subtotal));
        }

        public async Task<ApiResponse<bool>> MergeGuestCartAsync(int userId, string sessionId)
        {
            var guestCart = await repository.GetGuestCart(sessionId);
            if (guestCart is null)
                return ApiResponse<bool>.SuccessResponse(true, "No guest cart to merge");

            var userCart = await repository.GetUserCart(userId);

            if (userCart is null)
            {
                guestCart.UserId = userId;
                guestCart.SessionId = null;
                await repository.SaveChangesAsync();
                return ApiResponse<bool>.SuccessResponse(true, "Guest cart moved to account");
            }

            foreach (var guestItem in guestCart.Items)
            {
                var existing = userCart.Items.FirstOrDefault(i => i.ProductVariantId == guestItem.ProductVariantId);

                if (existing is not null)
                {
                    var variant = await repository.GetProductVariant(guestItem.ProductVariantId);
                    existing.Quantity = Math.Min(existing.Quantity + guestItem.Quantity, variant?.StockQuantity ?? existing.Quantity);
                }
                else
                {
                    repository.AddCartItem(new CartItem
                    {
                        CartId = userCart.Id,
                        ProductVariantId = guestItem.ProductVariantId,
                        Quantity = guestItem.Quantity,
                        IsSavedForLater = guestItem.IsSavedForLater,
                        AddedAt = DateTime.UtcNow
                    });
                }
            }

            repository.RemoveCart(guestCart);
            await repository.SaveChangesAsync();

            return ApiResponse<bool>.SuccessResponse(true, "Carts merged");
        }

        private CartSummaryDto BuildSummary(Cart cart, Coupon? coupon, decimal subtotal)
        {
            var freeShippingThreshold = configuration.GetValue<decimal>("Cart:FreeShippingThreshold");
            var standardShippingCost = configuration.GetValue<decimal>("Cart:StandardShippingCost");

            var shipping = subtotal >= freeShippingThreshold ? 0 : standardShippingCost;

            var discount = 0m;
            if (coupon is not null)
            {
                discount = coupon.DiscountType == DiscountType.Percentage
                    ? subtotal * (coupon.DiscountValue / 100)
                    : Math.Min(coupon.DiscountValue, subtotal);
            }

            return new CartSummaryDto
            {
                Subtotal = subtotal,
                Shipping = shipping,
                Discount = discount,
                Total = subtotal + shipping - discount,
                AppliedCouponCode = cart.AppliedCouponCode
            };
        }

        private static CartDto MapToDto(Cart cart)
        {
            return new CartDto
            {
                Id = cart.Id,
                Items = cart.Items.Select(i => new CartItemDto
                {
                    Id = i.Id,
                    ProductVariantId = i.ProductVariantId,
                    ProductName = i.ProductVariant.Product.Name,
                    Color = i.ProductVariant.Color,
                    Size = i.ProductVariant.Size,
                    Price = i.ProductVariant.Price,
                    Quantity = i.Quantity,
                    IsSavedForLater = i.IsSavedForLater,
                    StockWarning = i.ProductVariant.StockQuantity <= LowStockThreshold
                        ? $"Yalnız {i.ProductVariant.StockQuantity} ədəd qalıb"
                        : null
                }).ToList()
            };
        }
    }
}