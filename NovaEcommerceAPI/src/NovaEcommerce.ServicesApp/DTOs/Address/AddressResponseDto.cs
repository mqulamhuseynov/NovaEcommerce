using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.DTOs.Address
{
    public class AddressResponseDto
    {
        public int Id { get; set; }

        public string Label { get; set; } = default!;

        public string FullName { get; set; } = default!;

        public string AddressLine1 { get; set; } = default!;

        public string? AddressLine2 { get; set; }

        public string City { get; set; } = default!;

        public string State { get; set; } = default!;

        public string PostalCode { get; set; } = default!;

        public string Country { get; set; } = default!;

        public string Phone { get; set; } = default!;

        public bool IsDefault { get; set; }  
    }
}
