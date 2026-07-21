using NovaEcommerce.ServicesApp.DTOs.Order;
using NovaEcommerce.ServicesApp.DTOs.Responses;

namespace NovaEcommerce.ServicesApp.Services.Interfaces.Service;

public interface IOrderService
{
    Task<ApiResponse<OrderHistoryDto>> GetOrdersAsync(int userId, string? status, string? search, int page, int limit);
    Task<ApiResponse<OrderResponseDto>> GetOrderDetailAsync(int userId, string orderNumber);
    Task<ApiResponse<OrderTrackingDto>> GetTrackingAsync(int userId, string orderNumber);
    Task<ApiResponse<string>> ReorderAsync(int userId, int orderId);
    Task<ApiResponse<InvoiceDto>> GetInvoiceAsync(int userId, string orderNumber);
    Task<ApiResponse<bool>> AdvanceStatusAsync(string orderNumber);
}