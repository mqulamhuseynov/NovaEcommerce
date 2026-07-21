using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.DTOs
{
    public class PaginationDto
    {
        public int Page { get; set; }
        public int Limit { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
    }
}
