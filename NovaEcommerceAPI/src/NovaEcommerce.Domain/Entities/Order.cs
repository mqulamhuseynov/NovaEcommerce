using NovaEcommerce.Domain.Enums;

namespace NovaEcommerce.Domain.Entities;

public class Order
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public string OrderNumber { get; set; } = default!;
    public OrderStatus Status { get; set; }
    public decimal Subtotal { get; set; }
    public decimal ShippingCost { get; set; }
    public decimal Tax { get; set; }
    public decimal Discount { get; set; }
    public decimal Total { get; set; }

    // Nullable FK: guest checkout has no saved Address/PaymentMethod row to point to,
    // and a registered user's Address/PaymentMethod can later be edited or deleted.
    public int? ShippingAddressId { get; set; }
    public int? PaymentMethodId { get; set; }

    // Snapshots: immutable copy of what was actually used at purchase time, same idea
    // as the *_snapshot fields on OrderItem. This is the source of truth for display;
    // the FKs above are only a convenience link back to the live Address/PaymentMethod.
    public string ShippingAddressSnapshot { get; set; } = default!;
    public string? PaymentCardTypeSnapshot { get; set; }
    public string? PaymentLastFourSnapshot { get; set; }

    public string ShippingMethod { get; set; } = default!;
    public string? TrackingNumber { get; set; }
    public string? Carrier { get; set; }
    public DateTime PlacedAt { get; set; }
    public DateTime? EstimatedDeliveryStart { get; set; }
    public DateTime? EstimatedDeliveryEnd { get; set; }

    public AppUser? User { get; set; }
    public Address? ShippingAddress { get; set; }
    public PaymentMethod? PaymentMethod { get; set; }
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    public ICollection<OrderStatusHistory> StatusHistory { get; set; } = new List<OrderStatusHistory>();
}
