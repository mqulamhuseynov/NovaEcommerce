using NovaEcommerce.ServicesApp.DTOs.Responses;
using NovaEcommerce.ServicesApp.DTOs.Review;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.Services.Interfaces.Service
{
    public interface IReviewService
    {
        public Task<ApiResponse<ReviewDto>> CreateReview(int userId, CreateReviewDto request);
        public Task<ApiResponse<ProductReviewsResponseDto>> GetProductReviews(int productId);
    }
}
