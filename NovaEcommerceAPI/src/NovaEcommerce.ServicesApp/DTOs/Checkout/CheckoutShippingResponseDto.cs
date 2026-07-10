using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.DTOs.Checkout
{
    public class CheckoutShippingResponseDto
    {
        public int CheckoutSessionId { get; set; }      
        public string ShippingMethod { get; set; } = default!;
        public decimal ShippingCost { get; set; }
    }
}
