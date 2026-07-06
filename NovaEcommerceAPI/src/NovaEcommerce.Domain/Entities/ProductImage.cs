namespace NovaEcommerce.Domain.Entities;

public class ProductImage
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ImageUrl { get; set; } = default!;
    public int DisplayOrder { get; set; }
    public bool IsPrimary { get; set; }

    public Product Product { get; set; } = default!;
}
