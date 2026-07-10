using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.DTOs.Product
{
    public class ProductDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public decimal BasePrice { get; set; }
        public string CategoryName { get; set; } = default!;
        public string BrandName { get; set; } = default!;
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }
        public List<string> Images { get; set; } = new();
        public List<ProductVariantDto> Variants { get; set; } = new();
        public List<RelatedProductDto> RelatedProducts { get; set; } = new();
    }
}
