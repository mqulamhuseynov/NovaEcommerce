using NovaEcommerce.Domain.Enums;

namespace NovaEcommerce.Domain.Entities;

public class Review
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int UserId { get; set; }
    public int OrderItemId { get; set; }
    public int Rating { get; set; }
    public string? Title { get; set; }
    public string? ReviewText { get; set; }
    public FitRating? FitRating { get; set; }
    public List<string> Photos { get; set; } = new();
    public DateTime CreatedAt { get; set; }

    public Product Product { get; set; } = default!;
    public AppUser User { get; set; } = default!;
    public OrderItem OrderItem { get; set; } = default!;
}
