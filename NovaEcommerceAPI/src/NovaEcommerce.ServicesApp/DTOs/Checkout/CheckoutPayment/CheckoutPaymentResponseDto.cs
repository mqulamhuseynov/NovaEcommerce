using NovaEcommerce.Domain.Entities;
using NovaEcommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.DTOs.Checkout.CheckoutPayment
{
    public class CheckoutPaymentResponseDto
    {
        public int PaymentMethodId { get; set; }
        public string? PaymentType { get; set; } 
        public string? LastFourDigits { get; set; }
    }
}
