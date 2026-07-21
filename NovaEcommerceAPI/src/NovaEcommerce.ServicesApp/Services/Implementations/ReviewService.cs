using Microsoft.EntityFrameworkCore;
using NovaEcommerce.Domain.Entities;
using NovaEcommerce.ServicesApp.DTOs.Responses;
using NovaEcommerce.ServicesApp.DTOs.Review;
using NovaEcommerce.ServicesApp.Services.Interfaces.Repository;
using NovaEcommerce.ServicesApp.Services.Interfaces.Service;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.Services.Implementations
{
    public class ReviewService(IReviewRepository repository) : IReviewService
    {
        public async Task<ApiResponse<ReviewDto>> CreateReview(int userId, CreateReviewDto request)
        {
            if (request.Rating < 1 || request.Rating > 5)
                return ApiResponse<ReviewDto>.FailResponse("Rating must be between 1 and 5", 400);

            var orderItem = await repository.GetUserOrderItem(request.OrderItemId, userId);
            if (orderItem is null) return ApiResponse<ReviewDto>.FailResponse("Order item not found", 404);

            var hasReviewed = await repository.HasReviewed(request.OrderItemId);
            if (hasReviewed) return ApiResponse<ReviewDto>.FailResponse("You already reviewed this item", 409);

            var review = new Review
            {
                ProductId = orderItem.ProductVariant.ProductId,
                UserId = userId,
                OrderItemId = request.OrderItemId,
                Rating = request.Rating,
                Title = request.Title,
                ReviewText = request.ReviewText,
                FitRating = request.FitRating,
                Photos = request.Photos,
                CreatedAt = DateTime.UtcNow
            };

            repository.AddReview(review);

            try
            {
                await repository.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                return ApiResponse<ReviewDto>.FailResponse("You already reviewed this item", 409);
            }

            var dto = new ReviewDto
            {
                Id = review.Id,
                Rating = review.Rating,
                Title = review.Title,
                ReviewText = review.ReviewText,
                FitRating = review.FitRating,
                Photos = review.Photos,
                CreatedAt = review.CreatedAt
            };

            return ApiResponse<ReviewDto>.SuccessResponse(dto, "Review added", 201);
        }

        public async Task<ApiResponse<ProductReviewsResponseDto>> GetProductReviews(int productId)
        {
            var productExists = await repository.ProductExists(productId);
            if (!productExists)
                return ApiResponse<ProductReviewsResponseDto>.FailResponse("Product not found", 404);

            var reviews = await repository.GetReviews(productId);

            var response = new ProductReviewsResponseDto
            {
                AverageRating = reviews.Count != 0 ? Math.Round(reviews.Average(r => r.Rating), 1) : 0,
                TotalReviews = reviews.Count,

                Reviews = reviews.Select(r => new ReviewDto
                {
                    Id = r.Id,
                    Rating = r.Rating,
                    Title = r.Title,
                    ReviewText = r.ReviewText,
                    FitRating = r.FitRating,
                    Photos = r.Photos,
                    CreatedAt = r.CreatedAt
                }).ToList()
            };

            return ApiResponse<ProductReviewsResponseDto>.SuccessResponse(response);
        }
    }
}