namespace NovaEcommerce.ServicesApp.DTOs;

public class InvoiceDto
{
    public string OrderNumber { get; set; } = default!;

    public string CustomerName { get; set; } = default!;

    public decimal Subtotal { get; set; }

    public decimal Shipping { get; set; }

    public decimal Tax { get; set; }

    public decimal Discount { get; set; }

    public decimal Total { get; set; }

    public DateTime PlacedAt { get; set; }

    public List<OrderItemDto> Items { get; set; } = new();
}