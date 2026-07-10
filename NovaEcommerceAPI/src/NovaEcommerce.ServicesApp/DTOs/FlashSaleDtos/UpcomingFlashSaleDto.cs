using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.DTOs.FlashSaleDtos
{
    public class UpcomingFlashSaleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public DateTime StartsAt { get; set; }
        public DateTime EndsAt { get; set; }
        public List<FlashSaleItemDto> Items { get; set; } = new();
    }
}
