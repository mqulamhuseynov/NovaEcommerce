namespace NovaEcommerce.Domain.Entities;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public int ProductVariantId { get; set; }
    public string ProductNameSnapshot { get; set; } = default!;
    public string ColorSnapshot { get; set; } = default!;
    public string SizeSnapshot { get; set; } = default!;
    public decimal PriceSnapshot { get; set; }
    public int Quantity { get; set; }

    public Order Order { get; set; } = default!;
    public ProductVariant ProductVariant { get; set; } = default!;
    public Review? Review { get; set; }
    public ReturnRequest? ReturnRequest { get; set; }
}
