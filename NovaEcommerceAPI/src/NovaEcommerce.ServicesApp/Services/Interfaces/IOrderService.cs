using NovaEcommerce.ServicesApp.DTOs;

namespace NovaEcommerce.ServicesApp.Services.Interfaces;

public interface IOrderService
{
    Task<ApiResponseDto<OrderHistoryDto>> GetOrdersAsync(
        int userId,
        string? status,
        string? search,
        int page,
        int limit);

    Task<ApiResponseDto<OrderResponseDto>> GetOrderDetailAsync(
        int userId,
        string orderNumber);

    Task<ApiResponseDto<OrderTrackingDto>> GetTrackingAsync(
        int userId,
        string orderNumber);

    Task AdvanceStatusAsync(string orderNumber);

    Task<ApiResponseDto<string>> ReorderAsync(
        int userId,
        int orderId);

    Task<ApiResponseDto<InvoiceDto>> GetInvoiceAsync(
        int userId,
        string orderNumber);
}