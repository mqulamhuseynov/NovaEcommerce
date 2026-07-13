using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.DTOs.Checkout.CheckoutSummary
{
    public class CheckoutSummaryItemDto
    {
        public int ProductVariantId { get; set; }   
        public string ProductName { get; set; } = default!;
        public string ImageUrl { get; set; } = default!;
        public string Color { get; set; } = default!;   
        public string Size { get; set; } = default!;
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
