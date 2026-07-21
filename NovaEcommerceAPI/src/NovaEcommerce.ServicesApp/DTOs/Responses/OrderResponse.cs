using NovaEcommerce.ServicesApp.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.DTOs.Responses
{
    public class OrderResponseDto
    {
        public string OrderNumber { get; set; } = null!;
        public string Status { get; set; } = null!;
        public decimal Total { get; set; }
        public DateTime PlacedAt { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
    }
}
