using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.DTOs.Checkout.PlaceOrderConfirmation
{
    public class OrderShippingConfirmationDto
    {
        public string FullName { get; set; } = default!;

        public string Address { get; set; } = default!;
        public string PostalCode { get; set; } = default!;

        public string City { get; set; } = default!;

        public string Country { get; set; } = default!;
    }
}
