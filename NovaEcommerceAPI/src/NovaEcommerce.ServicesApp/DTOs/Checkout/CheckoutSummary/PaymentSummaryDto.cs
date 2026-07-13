using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.DTOs.Checkout.CheckoutSummary
{
    public class PaymentSummaryDto
    {
        public string PaymentType { get; set; } = default!;
        public string? CartType { get; set; }
        public string? LastFourDigits{ get; set; }
    }
}
