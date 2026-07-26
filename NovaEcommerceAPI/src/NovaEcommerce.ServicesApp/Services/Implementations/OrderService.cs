using NovaEcommerce.Domain.Entities;
using NovaEcommerce.Domain.Enums;
using NovaEcommerce.ServicesApp.DTOs;
using NovaEcommerce.ServicesApp.DTOs.Order;
using NovaEcommerce.ServicesApp.DTOs.Responses;
using NovaEcommerce.ServicesApp.Services.Interfaces.Repository;
using NovaEcommerce.ServicesApp.Services.Interfaces.Service;

namespace NovaEcommerce.ServicesApp.Services.Implementations;

public class OrderService(IOrderRepository repository, ICartRepository cartRepository) : IOrderService
{
    public async Task<ApiResponse<OrderHistoryDto>> GetOrdersAsync(
        int userId, string? status, string? search, int page, int limit)
    {
        page = page < 1 ? 1 : page;
        limit = limit is < 1 or > 100 ? 20 : limit;

        OrderStatus? parsedStatus = null;
        if (!string.IsNullOrWhiteSpace(status) && status.ToLower() != "all")
        {
            if (!Enum.TryParse<OrderStatus>(status, true, out var parsed))
                return ApiResponse<OrderHistoryDto>.FailResponse($"Invalid status value: {status}", 400);

            parsedStatus = parsed;
        }

        var (orders, totalCount) = await repository.GetOrders(userId, parsedStatus, search, page, limit);

        var result = new OrderHistoryDto
        {
            Orders = orders.Select(MapToResponseDto).ToList(),
            Pagination = new PaginationDto
            {
                Page = page,
                Limit = limit,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling((double)totalCount / limit)
            }
        };

        return ApiResponse<OrderHistoryDto>.SuccessResponse(result, "Orders fetched successfully");
    }

    public async Task<ApiResponse<OrderResponseDto>> GetOrderDetailAsync(int userId, string orderNumber)
    {
        var order = await repository.GetOrderDetail(userId, orderNumber);
        if (order is null)
            return ApiResponse<OrderResponseDto>.FailResponse("Order not found", 404);

        return ApiResponse<OrderResponseDto>.SuccessResponse(MapToResponseDto(order), "Order detail");
    }

    public async Task<ApiResponse<OrderTrackingDto>> GetTrackingAsync(int userId, string orderNumber)
    {
        var order = await repository.GetOrderTracking(userId, orderNumber);
        if (order is null)
            return ApiResponse<OrderTrackingDto>.FailResponse("Order not found", 404);

        var timeline = order.StatusHistory
            .OrderBy(x => x.ChangedAt)
            .Select(x => new TrackingStepDto
            {
                Status = x.Status.ToString(),
                Date = x.ChangedAt,
                Completed = true,
                Active = x.Status == order.Status
            })
            .ToList();

        var result = new OrderTrackingDto { OrderNumber = order.OrderNumber, Timeline = timeline };
        return ApiResponse<OrderTrackingDto>.SuccessResponse(result, "Tracking information");
    }

    public async Task<ApiResponse<string>> ReorderAsync(int userId, int orderId)
    {
        var order = await repository.GetOrderForReorder(userId, orderId);
        if (order is null)
            return ApiResponse<string>.FailResponse("Order not found", 404);

        var cart = await cartRepository.GetOrCreateCart(userId, null);

        var skippedItems = new List<string>();

        foreach (var item in order.Items)
        {
            var variant = await cartRepository.GetProductVariant(item.ProductVariantId);

            if (variant is null || variant.StockQuantity < item.Quantity)
            {
                skippedItems.Add(item.ProductNameSnapshot);
                continue;
            }

            var existing = await cartRepository.GetCartItemByVariant(cart.Id, item.ProductVariantId);

            if (existing is not null)
            {
                existing.Quantity += item.Quantity;
            }
            else
            {
                cartRepository.AddCartItem(new CartItem
                {
                    CartId = cart.Id,
                    ProductVariantId = item.ProductVariantId,
                    Quantity = item.Quantity,
                    AddedAt = DateTime.UtcNow
                });
            }
        }

        await cartRepository.SaveChangesAsync();

        var message = skippedItems.Count == 0
            ? "All items added to cart"
            : $"Added to cart. Unavailable: {string.Join(", ", skippedItems)}";

        return ApiResponse<string>.SuccessResponse("OK", message);
    }

    public async Task<ApiResponse<InvoiceDto>> GetInvoiceAsync(int userId, string orderNumber)
    {
        var order = await repository.GetOrderInvoice(userId, orderNumber);
        if (order is null)
            return ApiResponse<InvoiceDto>.FailResponse("Order not found", 404);

        var invoice = new InvoiceDto
        {
            OrderNumber = order.OrderNumber,
            CustomerName = $"{order.User!.FirstName} {order.User.LastName}",
            Subtotal = order.Subtotal,
            Shipping = order.ShippingCost,
            Tax = order.Tax,
            Discount = order.Discount,
            Total = order.Total,
            PlacedAt = order.PlacedAt,
            Items = order.Items.Select(i => new OrderItemDto
            {
                ProductName = i.ProductNameSnapshot,
                Color = i.ColorSnapshot,
                Size = i.SizeSnapshot,
                Price = i.PriceSnapshot,
                Quantity = i.Quantity
            }).ToList()
        };

        return ApiResponse<InvoiceDto>.SuccessResponse(invoice, "Invoice");
    }

    public async Task<ApiResponse<bool>> AdvanceStatusAsync(int userId, string orderNumber)
    {
        var order = await repository.GetOrderForUser(userId,orderNumber);
        if (order is null) 
        {
            return ApiResponse<bool>.FailResponse("order not found",404);
        }

        OrderStatus? nextStatus = order.Status switch
        {
            OrderStatus.Processing => OrderStatus.Shipped,
            OrderStatus.Shipped => OrderStatus.OutForDelivery,
            OrderStatus.OutForDelivery => OrderStatus.Delivered,
            _ => null
        };

        if (nextStatus is null)
            return ApiResponse<bool>.FailResponse("Order already completed", 400);

        order.Status = nextStatus.Value;

        repository.AddOrderStatusHistory(new OrderStatusHistory
        {
            OrderId = order.Id,
            Status = nextStatus.Value,
            ChangedAt = DateTime.UtcNow
        });

        await repository.SaveChangesAsync();

        return ApiResponse<bool>.SuccessResponse(true, $"Order advanced to {nextStatus}");
    }

    private static OrderResponseDto MapToResponseDto(Order order) => new()
    {
        OrderNumber = order.OrderNumber,
        Status = order.Status.ToString(),
        Total = order.Total,
        PlacedAt = order.PlacedAt,
        Items = order.Items.Select(i => new OrderItemDto
        {
            ProductName = i.ProductNameSnapshot,
            Color = i.ColorSnapshot,
            Size = i.SizeSnapshot,
            Price = i.PriceSnapshot,
            Quantity = i.Quantity
        }).ToList()
    };
}