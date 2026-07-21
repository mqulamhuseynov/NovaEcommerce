using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.DTOs.Order
{
    public class TrackingStepDto
    {
        public string Status { get; set; } = null!;
        public DateTime Date { get; set; }
        public bool Completed { get; set; }
        public bool Active { get; set; }
    }
}
