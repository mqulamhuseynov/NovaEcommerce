using NovaEcommerce.Domain.Entities;

namespace NovaEcommerce.ServicesApp.Services.Interfaces.Repository
{
    public interface IReviewRepository
    {
        public Task<OrderItem?> GetUserOrderItem(int orderItemId, int userId);
        public Task<bool> HasReviewed(int orderItemId);
        public Task<bool> ProductExists(int productId);
        public void AddReview(Review review);
        public Task<IReadOnlyCollection<Review>> GetReviews(int productId);
        public Task SaveChangesAsync();
    }
}