using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NovaEcommerce.ServicesApp.DTOs.CartDtos;
using NovaEcommerce.ServicesApp.DTOs.Responses;
using NovaEcommerce.ServicesApp.Services.Interfaces.Service;
using System.Security.Claims;

namespace NovaEcommerce.API.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartController(ICartService cartService) : ControllerBase
    {
        private const string SessionHeaderName = "X-Session-Id";

        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var (userId, sessionId) = ResolveIdentity();
            var result = await cartService.GetCartAsync(userId, sessionId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("items")]
        public async Task<IActionResult> AddItem([FromBody] AddCartItemRequestDto request)
        {
            var (userId, sessionId) = ResolveIdentity();
            var result = await cartService.AddItemAsync(userId, sessionId, request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("items/{id}")]
        public async Task<IActionResult> UpdateItemQuantity(int id, [FromBody] UpdateCartItemRequestQuantityDto request)
        {
            var (userId, sessionId) = ResolveIdentity();
            var result = await cartService.UpdateItemQuantityAsync(userId, sessionId, id, request.Quantity);
            return StatusCode(result.StatusCode, result);
        }

        [HttpDelete("items/{id}")]
        public async Task<IActionResult> RemoveItem(int id)
        {
            var (userId, sessionId) = ResolveIdentity();
            var result = await cartService.RemoveItemAsync(userId, sessionId, id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPatch("items/{id}/save-for-later")]
        public async Task<IActionResult> ToggleSaveForLater(int id)
        {
            var (userId, sessionId) = ResolveIdentity();
            var result = await cartService.ToggleSaveForLaterAsync(userId, sessionId, id);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("apply-coupon")]
        public async Task<IActionResult> ApplyCoupon([FromBody] ApplyCouponRequestDto request)
        {
            var (userId, sessionId) = ResolveIdentity();
            var result = await cartService.ApplyCouponAsync(userId, sessionId, request);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("summary")]
        public async Task<IActionResult> GetSummary()
        {
            var (userId, sessionId) = ResolveIdentity();
            var result = await cartService.GetSummaryAsync(userId, sessionId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("merge")]
        public async Task<IActionResult> MergeGuestCart([FromHeader(Name = SessionHeaderName)] string sessionId)
        {
            var (userId, _) = ResolveIdentity();

            if (userId is null)
            {
                var unauthorized = ApiResponse<bool>.FailResponse("Login required for this operation", 401);
                return StatusCode(unauthorized.StatusCode, unauthorized);
            }

            var result = await cartService.MergeGuestCartAsync(userId.Value, sessionId);
            return StatusCode(result.StatusCode, result);
        }

        private (int? userId, string? sessionId) ResolveIdentity()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
                return (userId, null);
            }

            var sessionId = Request.Headers[SessionHeaderName].FirstOrDefault();

            if (string.IsNullOrEmpty(sessionId))
            {
                sessionId = Guid.NewGuid().ToString();
                Response.Headers[SessionHeaderName] = sessionId;
            }

            return (null, sessionId);
        }
    }
}