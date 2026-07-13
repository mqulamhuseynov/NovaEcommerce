using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.DTOs.CartDtos
{
    public class CartDto
    {
        public int Id { get; set; }
        public ICollection<CartItemDto> Items { get; set; }
    }
}
