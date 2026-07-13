using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.DTOs.CartDtos
{
    public class AddCartItemRequestDto
    {
        public int ProductVariantId { get; set; }
        public int Quantity { get; set; }
    }
}
