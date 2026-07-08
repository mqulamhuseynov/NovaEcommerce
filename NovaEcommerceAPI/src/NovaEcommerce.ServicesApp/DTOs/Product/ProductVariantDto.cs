using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.DTOs.Product
{
    public class ProductVariantDto
    {
        public int Id { get; set; }
        public string Color { get; set; } = default!;
        public string Size { get; set; } = default!;
        public string Sku { get; set; } = default!;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
    }
}
