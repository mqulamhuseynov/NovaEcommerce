using Microsoft.AspNetCore.Mvc;
using NovaEcommerce.ServicesApp.DTOs.Product;
using NovaEcommerce.ServicesApp.Services.Interfaces;
using NovaEcommerce.ServicesApp.Services.Interfaces.Service;

namespace NovaEcommerce.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts([FromQuery] ProductFilterDto filter)
    {
        var response = await _productService.GetPLP(filter);

        return response.StatusCode switch
        {
            200 => Ok(response),
            400 => BadRequest(response),
            404 => NotFound(response),
            _ => StatusCode(response.StatusCode, response)
        };
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductDetail(int id)
    {
        var response = await _productService.GetProductDetail(id);

        return response.StatusCode switch
        {
            200 => Ok(response),
            404 => NotFound(response),
            400 => BadRequest(response),
            _ => StatusCode(response.StatusCode, response)
        };
    }
}