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
    public class WishlistRepository(AppDbContext context) : IWishlistRepository
    {
        public void AddWishlist(Wishlist wishlist) => context.Wishlists.Add(wishlist);


        public void AddWishlistItem(WishlistItem wishlistItem) => context.WishlistItems.Add(wishlistItem);

        public async Task<ProductVariant?> GetProductVariant(int productVariantId) => await context.ProductVariants.Include(p => p.Product).FirstOrDefaultAsync(v => v.Id == productVariantId);


        public async Task<WishlistItem?> GetWishlistItem(int wishlistId, int itemId)
        {
            return await context.WishlistItems.Include(i => i.ProductVariant)
                .FirstOrDefaultAsync(i => i.WishlistId == wishlistId && i.Id == itemId);
        }

        public async Task<WishlistItem?> GetWishlistItemByVariant(int wishlistId, int productVariantId)
        {
            return await context.WishlistItems.FirstOrDefaultAsync(i => i.WishlistId == wishlistId && i.ProductVariantId == productVariantId);
        }

        public async Task<IReadOnlyCollection<Wishlist>> GetWishlists(int userId)
        {
            return await context.Wishlists.Where(i => i.UserId == userId).Include(i => i.Items).ToListAsync();
        }

        public async Task<Wishlist?> GetWishlistsById(int userId, int wishlistId)
        {
            return await context.Wishlists.Include(i=>i.Items)
                //bug
                .ThenInclude(i => i.ProductVariant)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(i => i.UserId == userId && i.Id == wishlistId);
        }

        public void RemoveWishlistItem(WishlistItem wishlistItem) => context.WishlistItems.Remove(wishlistItem);


        public async Task SaveChangesAsync() => await context.SaveChangesAsync();

    }
}
