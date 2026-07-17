namespace NovaEcommerce.Domain.Entities;

public class WishlistItem
{
    public int Id { get; set; }
    public int WishlistId { get; set; }
    public int ProductVariantId { get; set; }
    public DateTime AddedAt { get; set; }
    public bool NotifyRequested { get; set; }
    public decimal PriceAtAdd { get; set; }

    public Wishlist Wishlist { get; set; } = default!;
    public ProductVariant ProductVariant { get; set; } = default!;
}
