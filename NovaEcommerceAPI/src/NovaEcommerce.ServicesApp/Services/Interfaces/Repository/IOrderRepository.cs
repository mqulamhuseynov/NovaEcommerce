using NovaEcommerce.Domain.Entities;
using NovaEcommerce.Domain.Enums;

namespace NovaEcommerce.ServicesApp.Services.Interfaces.Repository;

public interface IOrderRepository
{
    Task<(IReadOnlyCollection<Order> Orders, int TotalCount)> GetOrders(
        int userId, OrderStatus? status, string? search, int page, int limit);

    Task<Order?> GetOrderDetail(int userId, string orderNumber);
    Task<Order?> GetOrderTracking(int userId, string orderNumber);
    Task<Order?> GetOrderForReorder(int userId, int orderId);
    Task<Order?> GetOrderInvoice(int userId, string orderNumber);
    Task<Order?> GetOrderForUser(int userId, string OrderNumber); 

    Task<Order?> GetOrderByNumber(string orderNumber);

    void AddOrderStatusHistory(OrderStatusHistory history);
    Task SaveChangesAsync();
}