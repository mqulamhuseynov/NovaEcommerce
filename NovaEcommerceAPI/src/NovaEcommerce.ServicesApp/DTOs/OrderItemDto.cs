namespace NovaEcommerce.ServicesApp.DTOs;

public class OrderItemDto
{
    public string ProductName { get; set; } = default!;
    public string Color { get; set; } = default!;
    public string Size { get; set; } = default!;

    public decimal Price { get; set; }

    public int Quantity { get; set; }
}