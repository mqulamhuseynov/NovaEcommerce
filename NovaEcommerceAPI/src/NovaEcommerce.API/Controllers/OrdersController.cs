using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NovaEcommerce.ServicesApp.Services.Interfaces.Service;
using System.Security.Claims;

namespace NovaEcommerce.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/orders")]
    public class OrderController(IOrderService orderService) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetOrders(
            [FromQuery] string? status, [FromQuery] string? search,
            [FromQuery] int page = 1, [FromQuery] int limit = 20)
        {
            var result = await orderService.GetOrdersAsync(GetUserId(), status, search, page, limit);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{orderNumber}")]
        public async Task<IActionResult> GetOrderDetail(string orderNumber)
        {
            var result = await orderService.GetOrderDetailAsync(GetUserId(), orderNumber);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{orderNumber}/tracking")]
        public async Task<IActionResult> GetTracking(string orderNumber)
        {
            var result = await orderService.GetTrackingAsync(GetUserId(), orderNumber);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("{orderId}/reorder")]
        public async Task<IActionResult> Reorder(int orderId)
        {
            var result = await orderService.ReorderAsync(GetUserId(), orderId);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("{orderNumber}/invoice")]
        public async Task<IActionResult> GetInvoice(string orderNumber)
        {
            var result = await orderService.GetInvoiceAsync(GetUserId(), orderNumber);
            return StatusCode(result.StatusCode, result);
        }

        [HttpPost("{orderNumber}/advance-status")]
        public async Task<IActionResult> AdvanceStatus(string orderNumber)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(userIdString, out int userId))
            {
                return Unauthorized();
            }
            var result = await orderService.AdvanceStatusAsync(userId,orderNumber);
            return StatusCode(result.StatusCode, result);
        }

        private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    }
}