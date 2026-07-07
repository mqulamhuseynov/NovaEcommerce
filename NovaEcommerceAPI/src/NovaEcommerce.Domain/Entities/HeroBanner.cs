using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.Domain.Entities
{
    public class HeroBanner
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;

        public string? LinkUrl { get; set; }

        public bool IsActive { get; set; }

        public int DisplayOrder { get; set; }
    }
}
