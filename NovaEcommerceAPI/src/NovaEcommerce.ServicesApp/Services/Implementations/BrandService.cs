using NovaEcommerce.DataAccess.Repositories.Interfaces;
using NovaEcommerce.Domain.Enums;
using NovaEcommerce.ServicesApp.DTOs.Brands;
using NovaEcommerce.ServicesApp.Services.Interfaces;

namespace NovaEcommerce.ServicesApp.Services.Implementations;

public class BrandService : IBrandService
{
    private readonly IBrandRepository _brandRepository;

    public BrandService(IBrandRepository brandRepository)
    {
        _brandRepository = brandRepository;
    }

    public async Task<BrandResponseDto?> GetBrandAsync(
        string slug,
        string? color,
        string? size,
        decimal? minPrice,
        decimal? maxPrice)
    {
        var brand = await _brandRepository.GetBrandAsync(slug);

        if (brand == null)
            return null;

        var products = brand.Products.AsQueryable();

        if (!string.IsNullOrWhiteSpace(color))
        {
            products = products.Where(x =>
                x.Variants.Any(v => v.Color == color));
        }

        if (!string.IsNullOrWhiteSpace(size))
        {
            products = products.Where(x =>
                x.Variants.Any(v => v.Size == size));
        }

        if (minPrice.HasValue)
        {
            products = products.Where(x =>
                x.BasePrice >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            products = products.Where(x =>
                x.BasePrice <= maxPrice.Value);
        }

        return new BrandResponseDto
        {
            Name = brand.Name,
            LogoUrl = brand.LogoUrl,
            Description = brand.Description,

            Products = products
                .Select(x => new ProductDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.BasePrice,
                    Image = x.Images
                        .Where(i => i.IsPrimary)
                        .Select(i => i.ImageUrl)
                        .FirstOrDefault()
                })
                .ToList(),

            BestSellers = products
                .Where(x => x.Tags.Any(t => t.TagType == TagType.BestSeller))
                .Select(x => new ProductDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.BasePrice,
                    Image = x.Images
                        .Where(i => i.IsPrimary)
                        .Select(i => i.ImageUrl)
                        .FirstOrDefault()
                })
                .ToList()
        };
    }
}