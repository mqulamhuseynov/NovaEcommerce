namespace NovaEcommerce.Domain.Entities;

// One row per (Color, Size) combination for a product — e.g. "Air Motion Sneakers /
// Arctic White / 42" and "Air Motion Sneakers / Arctic White / 43" are two separate
// variants, each with its own SKU, price and stock count.
public class ProductVariant
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string Color { get; set; } = default!;
    public string Size { get; set; } = default!;
    public string Sku { get; set; } = default!;
    public decimal Price { get; set; }
    public decimal? OriginalPrice { get; set; }
    public int StockQuantity { get; set; }
    public bool IsActive { get; set; }

    public Product Product { get; set; } = default!;
    public ICollection<FlashSaleItem> FlashSaleItems { get; set; } = new List<FlashSaleItem>();
    public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public ICollection<WishlistItem> WishlistItems { get; set; } = new List<WishlistItem>();
}
