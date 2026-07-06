namespace NovaEcommerce.Domain.Entities;

public class CartItem
{
    public int Id { get; set; }
    public int CartId { get; set; }
    public int ProductVariantId { get; set; }
    public int Quantity { get; set; }
    public bool IsSavedForLater { get; set; }
    public DateTime AddedAt { get; set; }

    public Cart Cart { get; set; } = default!;
    public ProductVariant ProductVariant { get; set; } = default!;
}
