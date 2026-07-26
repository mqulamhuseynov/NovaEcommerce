using Microsoft.EntityFrameworkCore;
using NovaEcommerce.DataAccess.DbContext;
using NovaEcommerce.Domain.Entities;
using NovaEcommerce.Domain.Enums;
using NovaEcommerce.ServicesApp.Services.Interfaces.Repository;

namespace NovaEcommerce.DataAccess.Repositories.Implementations;

public class OrderRepository(AppDbContext context) : IOrderRepository
{
    public async Task<(IReadOnlyCollection<Order> Orders, int TotalCount)> GetOrders(
        int userId, OrderStatus? status, string? search, int page, int limit)
    {
        var query = context.Orders
            .Include(o => o.Items)
            .Where(o => o.UserId == userId)
            .AsQueryable();

        if (status is not null)
            query = query.Where(o => o.Status == status);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(o =>
                o.OrderNumber.Contains(search) ||
                o.Items.Any(i => i.ProductNameSnapshot.Contains(search)));

        var totalCount = await query.CountAsync();

        var orders = await query
            .OrderByDescending(o => o.PlacedAt)
            .Skip((page - 1) * limit)
            .Take(limit)
            .ToListAsync();

        return (orders, totalCount);
    }

    public async Task<Order?> GetOrderDetail(int userId, string orderNumber)
    {
        return await context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.UserId == userId && o.OrderNumber == orderNumber);
    }

    public async Task<Order?> GetOrderTracking(int userId, string orderNumber)
    {
        return await context.Orders
            .Include(o => o.StatusHistory)
            .FirstOrDefaultAsync(o => o.UserId == userId && o.OrderNumber == orderNumber);
    }

    public async Task<Order?> GetOrderForReorder(int userId, int orderId)
    {
        return await context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
    }

    public async Task<Order?> GetOrderInvoice(int userId, string orderNumber)
    {
        return await context.Orders
            .Include(o => o.User)
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.UserId == userId && o.OrderNumber == orderNumber);
    }

    public async Task<Order?> GetOrderByNumber(string orderNumber)
    {
        return await context.Orders
            .Include(o => o.StatusHistory)
            .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);
    }

    public void AddOrderStatusHistory(OrderStatusHistory history) =>
        context.OrderStatusHistories.Add(history);

    public async Task SaveChangesAsync() => await context.SaveChangesAsync();

    public async Task<Order?> GetOrderForUser(int userId, string orderNumber)
    {
        return await context.Orders.FirstOrDefaultAsync(o => o.UserId == userId && o.OrderNumber == orderNumber);
    }
}