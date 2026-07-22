using NovaEcommerce.ServicesApp.DTOs.CartDtos;
using NovaEcommerce.ServicesApp.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.Services.Interfaces.Service
{
    public interface ICartService
    {
        Task<ApiResponse<CartDto>> GetCartAsync(int? userId, string? sessionId);
        Task<ApiResponse<CartDto>> AddItemAsync(int? userId, string? sessionId, AddCartItemRequestDto request);
        Task<ApiResponse<CartDto>> UpdateItemQuantityAsync(int? userId, string? sessionId, int cartItemId, int quantity);
        Task<ApiResponse<CartDto>> RemoveItemAsync(int? userId, string? sessionId, int cartItemId);
        Task<ApiResponse<CartDto>> ToggleSaveForLaterAsync(int? userId, string? sessionId, int cartItemId);
        Task<ApiResponse<CartSummaryDto>> ApplyCouponAsync(int? userId, string? sessionId, ApplyCouponRequestDto request);
        Task<ApiResponse<CartSummaryDto>> GetSummaryAsync(int? userId, string? sessionId);
        Task<ApiResponse<bool>> MergeGuestCartAsync(int userId, string sessionId);
    }
}