using Azure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage.Json;
using NovaEcommerce.ServicesApp.Services.Interfaces.Service;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Claims;

namespace NovaEcommerce.API.Controllers
{
    [Route("api/flash-sales")]
    [ApiController]
    public class FlashSalesController : ControllerBase
    {
        private readonly IFlashSaleService _service;

        public FlashSalesController(IFlashSaleService service)
        {
            _service = service;
        }

        [HttpGet("active")]
        public async Task<IActionResult> Active()
        {
            var result = await _service.TGetActiveAsync();

            return result.StatusCode switch
            {
                200 => Ok(result),
                400 => BadRequest(result),
                404 => NotFound(result),
                _ => StatusCode(result.StatusCode, result)
            };

        }

        [HttpGet("upcoming")]
        public async Task<IActionResult> Upcoming()
        {
            var result = await _service.TGetUpcomingAsync();

            return result.StatusCode switch
            {
                200 => Ok(result),
                400 => BadRequest(result),
                404 => NotFound(result),
                _ => StatusCode(result.StatusCode, result)
            };
        }

        [Authorize]
        [HttpPost("{id}/notify")]
        public async Task<IActionResult> Notify(int id)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;

            if(string.IsNullOrEmpty(email))
            {
                throw new UnauthorizedAccessException("User is not authenticated.");
            }

            var result = await _service.TNotifyAsync(id, email);

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
