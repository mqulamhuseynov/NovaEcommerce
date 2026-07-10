using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NovaEcommerce.ServicesApp.DTOs.Checkout;
using NovaEcommerce.ServicesApp.Services.Interfaces.Service;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace NovaEcommerce.API.Controllers
{
    [Route("api/checkout")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly ICheckoutShippingService _service;

        public CheckoutController(ICheckoutShippingService service)
        {
            _service = service;
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

            var result = await _service.SaveShippingAsync(request, userId, sessionId);

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
