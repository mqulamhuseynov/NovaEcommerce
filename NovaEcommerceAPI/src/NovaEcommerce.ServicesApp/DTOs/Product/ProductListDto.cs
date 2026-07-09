using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.DTOs.Product
{
    public class ProductListDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public decimal BasePrice { get; set; }
        public string PrimaryImageUrl { get; set; } = default!;
        public double AverageRating { get; set; }
        public int ReviewCount { get; set; }

        public IEnumerable<string> AvailableColors { get; set; } = Enumerable.Empty<string>();
        public IEnumerable<string> AvailableSizes { get; set; } = Enumerable.Empty<string>();
    }
}
