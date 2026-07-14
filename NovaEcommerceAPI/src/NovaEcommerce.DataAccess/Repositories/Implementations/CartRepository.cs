using Microsoft.EntityFrameworkCore;
using NovaEcommerce.DataAccess.DbContext;
using NovaEcommerce.Domain.Entities;
using NovaEcommerce.ServicesApp.Services.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.DataAccess.Repositories.Implementations
{
    public class CartRepository(AppDbContext context) : ICartRepository
    {
        public void AddCartItem(CartItem item) => context.CartItems.Add(item);

        public async Task<Coupon?> GetActiveCouponByCode(string code)
        {
            return await context.Coupons.FirstOrDefaultAsync(c => c.Code == code && c.IsActive);
        }

        public async Task<CartItem?> GetCartItem(int cartId, int cartItemId)
        {
            return await context.CartItems.FirstOrDefaultAsync(c => c.CartId == cartId && c.Id == cartItemId);
        }

        public async Task<CartItem?> GetCartItemByVariant(int cartId, int cartItemVariantId)
        {
            return await context.CartItems.FirstOrDefaultAsync(c => c.CartId == cartId && c.ProductVariantId == cartItemVariantId);
        }

        public async Task<Cart?> GetGuestCart(string sessionId)
        {
            return await context.Carts.Include(p => p.Items)
                .FirstOrDefaultAsync(u => u.SessionId == sessionId);
        }

        public async Task<Cart> GetOrCreateCart(int? userId, string? sessionId)
        {
            //sebeti get etmek ucun userid veya sessionid(guest) yoxlamasi edirik. Eger userId varsa, userin sebetini getiririk, yoxsa sessionId ile guest sebetini getiririk. Eger sebet tapilmazsa, yeni bir sebet yaradirig.
            Cart? cart = userId is not null ? await context.Carts.Include(p => p.Items)
                .ThenInclude(p => p.ProductVariant)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(u => u.UserId == userId) 
                : await context.Carts.Include(p => p.Items)
                .ThenInclude(p => p.ProductVariant)
                .ThenInclude(p => p.Product)
                .FirstOrDefaultAsync(u => u.SessionId == sessionId);

            if(cart is not null) return cart;

            cart = new Cart
            {
                UserId = userId,
                SessionId = userId is null ? sessionId : null,
            };

            context.Carts.Add(cart);
            await context.SaveChangesAsync();

            return cart;
        }

        public async Task<ProductVariant?> GetProductVariant(int productVariantId)
        {
            return await context.ProductVariants.FindAsync(productVariantId);
        }

        public async Task<Cart?> GetUserCart(int userId)
        {
            return await context.Carts.Include(p => p.Items)
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public void RemoveCart(Cart cart) => context.Carts.Remove(cart);


        public void RemoveCartItem(CartItem item) => context.CartItems.Remove(item);


        public async Task SaveChangesAsync() => await context.SaveChangesAsync();

    }
}
