using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.DTOs.CartDtos
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int ProductVariantId { get; set; }
        public string ProductName { get; set; } = default!;
        public string Color { get; set; } = default!;
        public string Size { get; set; } = default!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public bool IsSavedForLater { get; set; }
        public string? StockWarning { get; set; }
    }
}
