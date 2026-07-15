namespace NovaEcommerce.ServicesApp.DTOs;

public class OrderResponseDto
{
    public string OrderNumber { get; set; } = default!;
    public string Status { get; set; } = default!;
    public decimal Total { get; set; }
    public DateTime PlacedAt { get; set; }

    public List<OrderItemDto> Items { get; set; } = new();
}