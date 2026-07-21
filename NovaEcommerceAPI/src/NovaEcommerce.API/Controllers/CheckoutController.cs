using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NovaEcommerce.ServicesApp.DTOs.Checkout.CheckoutPayment;
using NovaEcommerce.ServicesApp.DTOs.Checkout.CheckoutShipping;
using NovaEcommerce.ServicesApp.Services.Interfaces.Service;
using System.Security.Claims;

namespace NovaEcommerce.API.Controllers
{
    [Route("api/checkout")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutShippingService _shippingService;
        private readonly ICheckoutPaymentService _paymentService;
        private readonly ICheckoutSummaryService _summaryService;
        private readonly IPlaceOrderService _placeOrderService;

        public CheckoutController(ICheckoutShippingService shippingService, ICheckoutPaymentService paymentService, ICheckoutSummaryService summaryService, IPlaceOrderService placeOrderService)
        {
            _shippingService = shippingService;
            _paymentService = paymentService;
            _summaryService = summaryService;
            _placeOrderService = placeOrderService;
        }

        [HttpPost("shipping")]
        public async Task<IActionResult> SaveShipping(CheckoutShippingRequestDto request)
        {
            int? userId = null;

            if (User.Identity?.IsAuthenticated == true)
            {
                userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            }

            var sessionId = Request.Headers["Session-Id"].FirstOrDefault();

            var result = await _shippingService.SaveShippingAsync(request, userId, sessionId);

            return result.StatusCode switch
            {
                200 => Ok(result),
                400 => BadRequest(result),
                404 => NotFound(result),
                _ => StatusCode(result.StatusCode, result)
            };
        }

        [Authorize]
        [HttpPost("payment")]
        public async Task<IActionResult> Payment(CheckoutPaymentRequestDto request)
        {
            int? userId = null;

            if (User.Identity?.IsAuthenticated == true)
            {
                userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            }

            var result = await _paymentService.SavePayment(request, userId);

            return result.StatusCode switch
            {
                200 => Ok(result),
                400 => BadRequest(result),
                404 => NotFound(result),
                _ => StatusCode(result.StatusCode, result)
            };
        }

        [HttpGet("summary/{checkoutSessionId}")]
        public async Task<IActionResult> GetSummary(int checkoutSessionId)
        {
            var result = await _summaryService.GetSummaryAsync(checkoutSessionId);

            return result.StatusCode switch
            {
                200 => Ok(result),
                400 => BadRequest(result),
                404 => NotFound(result),
                _ => StatusCode(result.StatusCode, result)  
            };
        }

        [Authorize]
        [HttpPost("place-order")]
        public async Task<IActionResult> PlaceOrder()
        {
            int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var result = await _placeOrderService.PlaceOrderAsync(userId);

            return result.StatusCode switch
            {
                200 => Ok(result),
                400 => BadRequest(result),
                404 => NotFound(result),
                _ => StatusCode(result.StatusCode, result)
            };
    }
    }
}
