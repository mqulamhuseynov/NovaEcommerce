using Microsoft.EntityFrameworkCore;
using NovaEcommerce.DataAccess.DbContext;
using NovaEcommerce.Domain.Entities;
using NovaEcommerce.ServicesApp.Services.Interfaces.Repository;

namespace NovaEcommerce.DataAccess.Repositories.Implementations
{
    public class ReviewRepository(AppDbContext context) : IReviewRepository
    {
        public void AddReview(Review review)
        {
            context.Reviews.Add(review);
        }

        public async Task<OrderItem?> GetUserOrderItem(int orderItemId, int userId)
        {
            return await context.OrderItems
                .Include(o => o.Order)
                .Include(o => o.ProductVariant)
                .FirstOrDefaultAsync(o => o.Id == orderItemId && o.Order.UserId == userId);
        }

        public async Task<IReadOnlyCollection<Review>> GetReviews(int productId)
        {
            return await context.Reviews.AsNoTracking().Where(r => r.ProductId == productId).ToListAsync();
        }

        public async Task<bool> HasReviewed(int orderItemId)
        {
            return await context.Reviews.AnyAsync(r => r.OrderItemId == orderItemId);
        }

        public async Task<bool> ProductExists(int productId)
        {
            return await context.Products.AnyAsync(p => p.Id == productId);
        }

        public async Task SaveChangesAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}