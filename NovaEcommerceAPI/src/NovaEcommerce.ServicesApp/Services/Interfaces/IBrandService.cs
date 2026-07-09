using NovaEcommerce.ServicesApp.DTOs.Brands;

namespace NovaEcommerce.ServicesApp.Services.Interfaces;

public interface IBrandService
{
    Task<BrandResponseDto?> GetBrandAsync(
        string slug,
        string? color,
        string? size,
        decimal? minPrice,
        decimal? maxPrice);
}