namespace NovaEcommerce.Domain.Entities;

public class Product
{
    public int Id { get; set; }
    public int BrandId { get; set; }
    public int CategoryId { get; set; }
    public string Name { get; set; } = default!;
    public string? Description { get; set; }
    public decimal BasePrice { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public Brand Brand { get; set; } = default!;
    public Category Category { get; set; } = default!;
    public ICollection<ProductVariant> Variants { get; set; } = new List<ProductVariant>();
    public ICollection<ProductImage> Images { get; set; } = new List<ProductImage>();
    public ICollection<ProductTag> Tags { get; set; } = new List<ProductTag>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}
