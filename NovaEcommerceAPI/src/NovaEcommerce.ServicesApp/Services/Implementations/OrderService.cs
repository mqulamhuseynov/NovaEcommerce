using Microsoft.EntityFrameworkCore;
using NovaEcommerce.DataAccess.DbContext;
using NovaEcommerce.Domain.Entities;
using NovaEcommerce.Domain.Enums;
using NovaEcommerce.ServicesApp.DTOs;
using NovaEcommerce.ServicesApp.Services.Interfaces;

namespace NovaEcommerce.ServicesApp.Services.Implementations;

public class OrderService : IOrderService
{
    private readonly AppDbContext _context;

    public OrderService(AppDbContext context)
    {
        _context = context;
    }
    public async Task<ApiResponseDto<OrderHistoryDto>> GetOrdersAsync(
    int userId,
    string? status,
    string? search,
    int page,
    int limit)
    {
        var query =
            _context.Orders
                .Include(x => x.Items)
                .Where(x => x.UserId == userId)
                .AsQueryable();

        if (!string.IsNullOrWhiteSpace(status) &&
            status.ToLower() != "all")
        {
            query =
                query.Where(x =>
                    x.Status.ToString().ToLower() ==
                    status.ToLower());
        }

        if (!string.IsNullOrWhiteSpace(search))
        {
            query =
                query.Where(x =>
                    x.OrderNumber.Contains(search) ||

                    x.Items.Any(i =>
                        i.ProductNameSnapshot.Contains(search)));
        }

        var totalCount =
            await query.CountAsync();

        var orders =
            await query
                .OrderByDescending(x => x.PlacedAt)
                .Skip((page - 1) * limit)
                .Take(limit)
                .ToListAsync();

        var data =
            orders.Select(x => new OrderResponseDto
            {
                OrderNumber = x.OrderNumber,
                Status = x.Status.ToString(),
                Total = x.Total,
                PlacedAt = x.PlacedAt,

                Items =
                    x.Items.Select(i => new OrderItemDto
                    {
                        ProductName = i.ProductNameSnapshot,
                        Color = i.ColorSnapshot,
                        Size = i.SizeSnapshot,
                        Price = i.PriceSnapshot,
                        Quantity = i.Quantity
                    }).ToList()

            }).ToList();

        return new ApiResponseDto<OrderHistoryDto>
        {
            Success = true,
            Message = "Orders fetched successfully",

            Data = new OrderHistoryDto
            {
                Orders = data,

                Pagination = new PaginationDto
                {
                    Page = page,
                    Limit = limit,
                    TotalCount = totalCount,
                    TotalPages =
                        (int)Math.Ceiling(
                            (double)totalCount / limit)
                }
            }
        };
    }
    public async Task<ApiResponseDto<OrderResponseDto>>
GetOrderDetailAsync(
    int userId,
    string orderNumber)
    {
        var order =
            await _context.Orders
                .Include(x => x.Items)
                .Include(x => x.StatusHistory)
                .FirstOrDefaultAsync(x =>
                    x.UserId == userId &&
                    x.OrderNumber == orderNumber);

        if (order == null)
            throw new Exception("Order not found.");

        var dto =
            new OrderResponseDto
            {
                OrderNumber = order.OrderNumber,
                Status = order.Status.ToString(),
                Total = order.Total,
                PlacedAt = order.PlacedAt,

                Items =
                    order.Items.Select(i => new OrderItemDto
                    {
                        ProductName = i.ProductNameSnapshot,
                        Color = i.ColorSnapshot,
                        Size = i.SizeSnapshot,
                        Price = i.PriceSnapshot,
                        Quantity = i.Quantity
                    }).ToList()
            };

        return new ApiResponseDto<OrderResponseDto>
        {
            Success = true,
            Message = "Order detail",

            Data = dto
        };
    }
    public async Task<ApiResponseDto<OrderTrackingDto>>
GetTrackingAsync(
    int userId,
    string orderNumber)
    {
        var order =
            await _context.Orders
                .Include(x => x.StatusHistory)
                .FirstOrDefaultAsync(x =>
                    x.UserId == userId &&
                    x.OrderNumber == orderNumber);

        if (order == null)
            throw new Exception("Order not found.");

        var timeline =
            order.StatusHistory
                .OrderBy(x => x.ChangedAt)
                .Select(x => new TrackingStepDto
                {
                    Status = x.Status.ToString(),
                    Date = x.ChangedAt,
                    Completed = true,
                    Active = x.Status == order.Status
                })
                .ToList();

        return new ApiResponseDto<OrderTrackingDto>
        {
            Success = true,
            Message = "Tracking information",

            Data = new OrderTrackingDto
            {
                OrderNumber = order.OrderNumber,
                Timeline = timeline
            }
        };
    }
    public async Task AdvanceStatusAsync(string orderNumber)
    {
        var order = await _context.Orders
            .Include(x => x.StatusHistory)
            .FirstOrDefaultAsync(x => x.OrderNumber == orderNumber);

        if (order == null)
            throw new Exception("Order not found.");

        OrderStatus? nextStatus = order.Status switch
        {
            OrderStatus.Processing => OrderStatus.Shipped,
            OrderStatus.Shipped => OrderStatus.OutForDelivery,
            OrderStatus.OutForDelivery => OrderStatus.Delivered,
            _ => null
        };

        if (nextStatus == null)
            throw new Exception("Order already completed.");

        order.Status = nextStatus.Value;

        _context.OrderStatusHistories.Add(new OrderStatusHistory
        {
            OrderId = order.Id,
            Status = nextStatus.Value,
            ChangedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();
    }
    public async Task<ApiResponseDto<string>> ReorderAsync(
    int userId,
    int orderId)
    {
        var order = await _context.Orders
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x =>
                x.Id == orderId &&
                x.UserId == userId);

        if (order == null)
            throw new Exception("Order not found.");

        var cart = await _context.Carts
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.UserId == userId);

        if (cart == null)
        {
            cart = new Cart
            {
                UserId = userId
            };

            _context.Carts.Add(cart);

            await _context.SaveChangesAsync();
        }

        foreach (var item in order.Items)
        {
            var variant =
                await _context.ProductVariants
                    .FirstOrDefaultAsync(x =>
                        x.Id == item.ProductVariantId);

            if (variant == null)
                continue;

            if (variant.StockQuantity < item.Quantity)
                continue;

            var existing =
                cart.Items.FirstOrDefault(x =>
                    x.ProductVariantId == item.ProductVariantId);

            if (existing == null)
            {
                cart.Items.Add(new CartItem
                {
                    ProductVariantId = item.ProductVariantId,
                    Quantity = item.Quantity,
                    AddedAt = DateTime.UtcNow
                });
            }
            else
            {
                existing.Quantity += item.Quantity;
            }
        }

        await _context.SaveChangesAsync();

        return new ApiResponseDto<string>
        {
            Success = true,
            Message = "Products added to cart.",
            Data = "OK"
        };
    }
    public async Task<ApiResponseDto<InvoiceDto>>
GetInvoiceAsync(
    int userId,
    string orderNumber)
    {
        var order =
            await _context.Orders
                .Include(x => x.User)
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x =>
                    x.UserId == userId &&
                    x.OrderNumber == orderNumber);

        if (order == null)
            throw new Exception("Order not found.");

        var invoice =
            new InvoiceDto
            {
                OrderNumber = order.OrderNumber,

                CustomerName =
                    order.User!.FirstName + " " +
                    order.User.LastName,

                Subtotal = order.Subtotal,
                Shipping = order.ShippingCost,
                Tax = order.Tax,
                Discount = order.Discount,
                Total = order.Total,
                PlacedAt = order.PlacedAt,

                Items =
                    order.Items.Select(i =>
                        new OrderItemDto
                        {
                            ProductName = i.ProductNameSnapshot,
                            Color = i.ColorSnapshot,
                            Size = i.SizeSnapshot,
                            Price = i.PriceSnapshot,
                            Quantity = i.Quantity
                        }).ToList()
            };

        return new ApiResponseDto<InvoiceDto>
        {
            Success = true,
            Message = "Invoice",

            Data = invoice
        };
    }
}