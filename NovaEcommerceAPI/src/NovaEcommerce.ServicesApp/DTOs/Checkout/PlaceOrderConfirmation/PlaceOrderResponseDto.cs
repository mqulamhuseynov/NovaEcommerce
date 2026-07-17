using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.DTOs.Checkout.PlaceOrderConfirmation
{
    public class PlaceOrderResponseDto
    {
        public string OrderNumber { get; set; } = default!;
        public DateTime EstimatedDeliveryStart { get; set; }
        public DateTime EstimatedDeliveryEnd { get; set; }
        public OrderShippingConfirmationDto Shipping { get; set; } = default!;
        public List<OrderItemConfirmationDto> Items { get; set; } = new();
        public OrderPriceConfirmationDto PriceConfirmation { get; set; } = default!;
    }
}
