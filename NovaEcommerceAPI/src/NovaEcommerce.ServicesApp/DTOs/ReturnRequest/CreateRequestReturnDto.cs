using NovaEcommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.DTOs.ReturnRequest
{
    public class CreateRequestReturnDto
    {
        public int OrderItemId { get; set; }
        public string Reason { get; set; } = default!;
        public ResolutionType ResolutionType { get; set; }
        public string? ExchangeSize { get; set; }
        public string? AdditionalNotes { get; set; }
        public List<string> Photos { get; set; } = new();
    }
}
