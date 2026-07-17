using NovaEcommerce.ServicesApp.DTOs.Responses;
using NovaEcommerce.ServicesApp.DTOs.WishlistDtos;


namespace NovaEcommerce.ServicesApp.Services.Interfaces.Service
{
    public interface IWishlistService
    {
        Task<ApiResponse<IReadOnlyCollection<WishlistDto>>> GetWishlists(int userId);
        Task<ApiResponse<WishlistDto>> CreateWishlist(int userId, CreateWishlistRequestDto request);
        Task<ApiResponse<WishlistItemDto>> AddItem(int userId, int wishlistId, AddWishlistItemRequestDto request);
        Task<ApiResponse<IReadOnlyCollection<WishlistItemDto>>> GetWishlistItems(int userId, int wishlistId);
        Task<ApiResponse<List<WishlistItemDto>>> RemoveWishlistItem(int userId, int wishlistId, int itemId);
        Task<ApiResponse<bool>> RequestNotify(int userId, int wishlistId, int itemId);
        Task<ApiResponse<WishlistShareUrlDto>> ShareWishlistUrl(int userId, int wishlistId);
    }
}
