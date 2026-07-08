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
    [Route("api/[controller]")]
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
            return Ok(result);
        }

        [HttpGet("upcoming")]
        public async Task<IActionResult> Upcoming()
        {
            var result = await _service.TGetUpcomingAsync();
            return Ok(result);
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

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
