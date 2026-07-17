using NovaEcommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.Services.Interfaces.Repository
{
    public interface IWishlistRepository
    {
        public Task<IReadOnlyCollection<Wishlist>> GetWishlists(int userId);
        public Task<Wishlist?> GetWishlistsById(int userId, int wishlistId);
        public Task<WishlistItem?> GetWishlistItem(int wishlistId, int itemId);
        public Task<WishlistItem?> GetWishlistItemByVariant(int wishlistId, int productVariantId);
        Task<ProductVariant?> GetProductVariant(int productVariantId);

        public void AddWishlist(Wishlist wishlist);
        public void AddWishlistItem(WishlistItem wishlistItem);
        public void RemoveWishlistItem(WishlistItem wishlistItem);

        Task SaveChangesAsync();
    }
}
