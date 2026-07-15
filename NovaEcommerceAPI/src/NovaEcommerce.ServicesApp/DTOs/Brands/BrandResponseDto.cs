namespace NovaEcommerce.ServicesApp.DTOs.Brands;

public class BrandResponseDto
{
    public string Name { get; set; } = default!;
    public string? LogoUrl { get; set; }
    public string? Description { get; set; }

    public List<ProductDto> Products { get; set; } = new();
    public List<ProductDto> BestSellers { get; set; } = new();
}