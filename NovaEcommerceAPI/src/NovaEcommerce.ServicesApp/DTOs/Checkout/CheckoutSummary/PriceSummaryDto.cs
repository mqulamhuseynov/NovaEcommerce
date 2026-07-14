using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.DTOs.Checkout.CheckoutSummary
{
    public class PriceSummaryDto
    {
        public decimal Subtotal { get; set; }   
        public decimal Shipping { get; set; }
        public decimal Tax { get; set; }    
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
    }
}
