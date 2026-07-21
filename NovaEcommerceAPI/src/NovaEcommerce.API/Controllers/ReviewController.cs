using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NovaEcommerce.ServicesApp.DTOs.Review;
using NovaEcommerce.ServicesApp.Services.Interfaces.Service;
using System.Security.Claims;

namespace NovaEcommerce.API.Controllers
{
    [ApiController]
    public class ReviewController(IReviewService reviewService) : ControllerBase
    {
        [Authorize]
        [HttpPost("api/reviews")]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewDto request)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await reviewService.CreateReview(userId, request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("api/products/{id}/reviews")]
        public async Task<IActionResult> GetProductReviews(int id)
        {
            var result = await reviewService.GetProductReviews(id);
            return StatusCode(result.StatusCode, result);
        }
    }
}