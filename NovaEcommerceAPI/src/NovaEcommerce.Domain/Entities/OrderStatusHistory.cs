using NovaEcommerce.Domain.Enums;

namespace NovaEcommerce.Domain.Entities;

public class OrderStatusHistory
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime ChangedAt { get; set; }

    public Order Order { get; set; } = default!;
}
