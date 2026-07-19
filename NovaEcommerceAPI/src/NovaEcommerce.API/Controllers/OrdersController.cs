using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NovaEcommerce.ServicesApp.Services.Interfaces;
using System.Security.Claims;

namespace NovaEcommerce.API.Controllers;

[ApiController]
[Authorize]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders(
        string? status,
        string? search,
        int page = 1,
        int limit = 10)
    {
        var userId =
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result =
            await _orderService.GetOrdersAsync(
                userId,
                status,
                search,
                page,
                limit);

        return Ok(result);
    }

    [HttpGet("{orderNumber}")]
    public async Task<IActionResult> GetOrderDetail(
        string orderNumber)
    {
        var userId =
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result =
            await _orderService.GetOrderDetailAsync(
                userId,
                orderNumber);

        return Ok(result);
    }

    [HttpGet("{orderNumber}/tracking")]
    public async Task<IActionResult> GetTracking(
        string orderNumber)
    {
        var userId =
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result =
            await _orderService.GetTrackingAsync(
                userId,
                orderNumber);

        return Ok(result);
    }

    [HttpPatch("{orderNumber}/advance-status")]
    public async Task<IActionResult> AdvanceStatus(
        string orderNumber)
    {
        await _orderService.AdvanceStatusAsync(orderNumber);

        return Ok(new
        {
            success = true,
            message = "Status updated successfully."
        });
    }

    [HttpPost("{orderId}/reorder")]
    public async Task<IActionResult> Reorder(
        int orderId)
    {
        var userId =
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result =
            await _orderService.ReorderAsync(
                userId,
                orderId);

        return Ok(result);
    }

    [HttpGet("{orderNumber}/invoice")]
    public async Task<IActionResult> Invoice(
        string orderNumber)
    {
        var userId =
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result =
            await _orderService.GetInvoiceAsync(
                userId,
                orderNumber);

        return Ok(result);
    }
}