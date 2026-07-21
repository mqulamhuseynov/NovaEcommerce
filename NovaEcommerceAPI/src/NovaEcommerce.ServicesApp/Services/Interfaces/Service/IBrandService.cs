using NovaEcommerce.ServicesApp.DTOs.Brands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.Services.Interfaces.Service
{
    public interface IBrandService
    {
        public Task<BrandResponseDto?> GetBrandAsync(
        string slug,
        string? color,
        string? size,
        decimal? minPrice,
        decimal? maxPrice);


    }
}
