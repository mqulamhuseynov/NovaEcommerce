using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.DTOs.FlashSaleDtos
{
    public class FlashSaleItemDto
    {
        public int Id { get; set; }
        public int ProductVariantId { get; set; }
        public string ProductName { get; set; } = default!; 
        public string ImageUrl { get; set; } = default!;    
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal DiscountPercentage { get; set; }
        public int SoldCount { get; set; }
        public int StockLimit { get; set; }
        public decimal SoldPercentage =>
            Math.Round(StockLimit == 0 ? 0 : (decimal)SoldCount / StockLimit * 100);
    }
}
