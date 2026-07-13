using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.DTOs.Checkout.CheckoutSummary
{
    public class CheckoutSummaryResponseDto
    {
        public ShippingSummaryDto Shipping { get; set; } = default!;

        public PaymentSummaryDto Payment { get; set; } = default!;

        public List<CheckoutSummaryItemDto> Items { get; set; } = new();

        public PriceSummaryDto Price { get; set; } = default!;      

    }
}
