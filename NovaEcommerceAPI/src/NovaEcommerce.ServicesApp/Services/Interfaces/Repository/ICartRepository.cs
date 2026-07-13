using NovaEcommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.Services.Interfaces.Repository
{
    public interface ICartRepository
    {
        public Task<Cart> GetOrCreateCart(int? userId, string? sessionId);
        public Task<CartItem?> GetCartItem(int cartId, int cartItemId);
        public Task<CartItem?> GetCartItemByVariant(int cartId, int cartItemVariantId);
        public Task<ProductVariant?> GetProductVariant(int productVariantId);
        public Task<Coupon?> GetActiveCouponByCode(string code);
        public Task<Cart?> GetGuestCart(string sessionId);
        public Task<Cart?> GetUserCart(int userId);
        public void AddCartItem(CartItem item);
        public void RemoveCartItem(CartItem item);
        public void RemoveCart(Cart cart);
        public Task SaveChangesAsync();
    }
}
