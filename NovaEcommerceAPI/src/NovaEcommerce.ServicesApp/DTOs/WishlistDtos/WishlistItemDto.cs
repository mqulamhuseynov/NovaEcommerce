using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.DTOs.WishlistDtos
{
    public class WishlistItemDto
    {
        public int Id { get; set; }
        public int ProductVariantId { get; set; }
        public string ProductName { get; set; } = null!;
        public string Color { get; set; } = null!;
        public string Size { get; set; } = null!;
        public decimal CurrentPrice { get; set; }
        public decimal PriceAtAdd { get; set; }
        public bool PriceDropped { get; set; }
        public bool OutOfStock { get; set; }
        public bool BackInStock { get; set; }
        public bool NotifyRequested { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
