using NovaEcommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.DTOs.ReturnRequest
{
    public class ReturnRequestDto
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = default!;
        public string Color { get; set; } = default!;   
        public string Size { get; set; } = default!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Reason { get; set; } = default!;
        public ResolutionType ResolutionType { get; set; }
        public string? ExchangeSize { get; set; }
        public string? AdditionalNotes { get; set; }
        public List<string> Photos { get; set; } = new();
        public ReturnStatus Status { get; set; }  
        public DateTime CreatedAt { get; set; }

    }
}
