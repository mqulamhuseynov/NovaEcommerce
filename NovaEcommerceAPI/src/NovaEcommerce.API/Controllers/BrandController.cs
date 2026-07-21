using Microsoft.AspNetCore.Mvc;
using NovaEcommerce.ServicesApp.Services.Interfaces;
using NovaEcommerce.ServicesApp.Services.Interfaces.Service;

namespace NovaEcommerce.API.Controllers;

[ApiController]
[Route("api/brands")]
public class BrandController : ControllerBase
{
    private readonly IBrandService _brandService;

    public BrandController(IBrandService brandService)
    {
        _brandService = brandService;
    }

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetBrand(
        string slug,
        [FromQuery] string? color,
        [FromQuery] string? size,
        [FromQuery] decimal? minPrice,
        [FromQuery] decimal? maxPrice)
    {
        var result = await _brandService.GetBrandAsync(
            slug,
            color,
            size,
            minPrice,
            maxPrice);

        if (result == null)
        {
            return NotFound(new
            {
                success = false,
                message = "Brand not found"
            });
        }

        return Ok(new
        {
            success = true,
            message = "Success",
            data = result
        });
    }
}