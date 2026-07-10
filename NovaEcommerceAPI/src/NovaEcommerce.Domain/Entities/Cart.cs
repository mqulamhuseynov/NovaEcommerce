namespace NovaEcommerce.Domain.Entities;

public class Cart
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public string? SessionId { get; set; }

    public string? AppliedCouponCode { get; set; }

    public AppUser? User { get; set; }
    public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
}
