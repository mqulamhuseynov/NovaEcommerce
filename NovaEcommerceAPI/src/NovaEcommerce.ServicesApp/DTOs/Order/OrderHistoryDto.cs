using NovaEcommerce.ServicesApp.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.DTOs.Order
{
    public class OrderHistoryDto
    {
        public List<OrderResponseDto> Orders { get; set; } = new();
        public PaginationDto Pagination { get; set; } = null!;
    }
}
