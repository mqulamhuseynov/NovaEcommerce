using NovaEcommerce.Domain.Enums;

namespace NovaEcommerce.Domain.Entities;

public class ProductTag
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public TagType TagType { get; set; }
    public string? Value { get; set; }

    public Product Product { get; set; } = default!;
}
