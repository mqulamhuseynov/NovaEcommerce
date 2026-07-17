using NovaEcommerce.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.Domain.Entities
{
    public class CheckoutSession
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string? SessionId { get; set; }  
        public string Email { get; set; } = default!;   
        public string FirstName { get; set; } = default!;
        public string LastName { get; set; } = default!;
        public string AddressLine1 { get; set; } = default!;
        public string? AddressLine2 { get; set; }
        public string City { get; set; } = default!;
        public string State { get; set; } = default!;
        public string PostalCode { get; set; } = default!;
        public string Country { get; set; } = default!;
        public string Phone { get; set; } = default!;
        public ShippingMethod ShippingMethod { get; set; }
        public decimal ShippingCost { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public PaymentType? PaymentType { get; set; }
        public int? PaymentMethodId { get; set; }    
        public PaymentMethod? PaymentMethod { get; set; }   

        public AppUser? User { get; set; }
    }
}
