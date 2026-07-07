using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NovaEcommerce.ServicesApp.Services.Interfaces;

namespace NovaEcommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IHomeService _service;

        public HomeController(IHomeService service)
        {
            _service = service;
        }

        [HttpGet("hero-banners")]
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
