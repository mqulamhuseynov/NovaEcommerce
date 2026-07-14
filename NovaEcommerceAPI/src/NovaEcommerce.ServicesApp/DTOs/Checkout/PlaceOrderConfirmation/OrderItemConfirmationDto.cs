using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.DTOs.Checkout.PlaceOrderConfirmation
{
    public class OrderItemConfirmationDto
    {
        public string ProductName { get; set; } = default!;
        public string? ImageUrl { get; set; }  
        public string Color { get; set; } = default!;   
        public string Size { get; set; } = default!;    
        public decimal Price { get; set; }
        public int Quantity { get; set; }   
    }
}
