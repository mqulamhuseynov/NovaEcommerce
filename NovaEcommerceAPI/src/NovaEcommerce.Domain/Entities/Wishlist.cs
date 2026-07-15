namespace NovaEcommerce.Domain.Entities;

public class Wishlist
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = "All items";

    public AppUser User { get; set; } = default!;
    public ICollection<WishlistItem> Items { get; set; } = new List<WishlistItem>();
}
