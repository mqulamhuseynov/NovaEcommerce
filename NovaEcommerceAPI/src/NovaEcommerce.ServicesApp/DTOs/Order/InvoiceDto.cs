using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.DTOs.Order
{
    public class InvoiceDto
    {
        public string OrderNumber { get; set; } = null!;
        public string CustomerName { get; set; } = null!;
        public decimal Subtotal { get; set; }
        public decimal Shipping { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public DateTime PlacedAt { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }
}
