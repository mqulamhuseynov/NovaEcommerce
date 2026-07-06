using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.Domain.Entities.Commons
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
