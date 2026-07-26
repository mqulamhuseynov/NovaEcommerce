using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NovaEcommerce.ServicesApp.DTOs;
using NovaEcommerce.ServicesApp.DTOs.Responses;
using NovaEcommerce.ServicesApp.Services.Interfaces.Service;

namespace NovaEcommerce.API.Controllers
{
    [Route("api/home")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHomeService _service;

        public HomeController(IHomeService service)
        {
            _service = service;
        }

        [HttpGet("hero-banners")]
        [ProducesResponseType(typeof(ApiResponse<HeroBannerDto>), 200)]
        [ProducesResponseType(typeof(ApiResponse<HeroBannerDto>), 404)]
        public async Task<IActionResult> GetHeroBanners()
        {
            var result = await _service.TGetHeroBannersAsync();
            return Ok(result);  
        }

        [HttpGet("curated-picks")]
        public async Task<IActionResult> GetCuratedPicks()
        {
            var result = await _service.TGetCuratedPicksAsync();
            return Ok(result);
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var result = await _service.TGetCategoriesAsync();
            return Ok(result);
        }
    }
}
