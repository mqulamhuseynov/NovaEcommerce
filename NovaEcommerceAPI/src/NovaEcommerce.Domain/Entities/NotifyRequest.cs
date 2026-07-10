using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.Domain.Entities
{
    public class NotifyRequest
    {
        public int Id { get; set; }

        public int FlashSaleId { get; set; }

        public string Email { get; set; } = default!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public FlashSale FlashSale { get; set; } = default!;
    }
}
