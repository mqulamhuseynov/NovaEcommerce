using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.DTOs.Checkout.CheckoutPayment
{
    public class CheckoutPaymentRequestDto
    {
        public int CheckoutSessionId { get; set; }
        public int PaymentType { get; set; }
        public string? CardNumber { get; set; }  
        public string? CVV{ get; set; }
        public int? ExpiryMonth { get; set; }
        public int? ExpiryYear { get; set; }
        public string? CardHolderName { get; set; } 

    }
}
