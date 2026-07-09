using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.DTOs.Product
{
    public class ProductFilterDto
    {
        public string? CategorySlug { get; set; }
        public string? Color { get; set; }
        public string? Size { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public string Sort { get; set; } = "newest"; // relevance, price_asc, price_desc, newest
        public int Page { get; set; } = 1;
        public int Limit { get; set; } = 10;
    }
}
