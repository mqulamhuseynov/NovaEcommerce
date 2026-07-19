using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NovaEcommerce.ServicesApp.DTOs.PaymentMethods;
using NovaEcommerce.ServicesApp.Services.Interfaces;
using System.Security.Claims;

namespace NovaEcommerce.API.Controllers;

[ApiController]
[Authorize]
[Route("api/payment-methods")]
public class PaymentMethodsController : ControllerBase
{
    private readonly IPaymentMethodService _paymentMethodService;

    public PaymentMethodsController(
        IPaymentMethodService paymentMethodService)
    {
        _paymentMethodService = paymentMethodService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var userId =
            int.Parse(User.FindFirstValue(
                ClaimTypes.NameIdentifier)!);

        var result =
            await _paymentMethodService.GetAllAsync(userId);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        CreatePaymentMethodDto dto)
    {
        var userId =
            int.Parse(User.FindFirstValue(
                ClaimTypes.NameIdentifier)!);

        await _paymentMethodService.AddAsync(
            userId,
            dto);

        return Ok(new
        {
            success = true,
            message = "Payment method added successfully."
        });
    }

    [HttpPatch("{id}/set-default")]
    public async Task<IActionResult> SetDefault(
        int id)
    {
        var userId =
            int.Parse(User.FindFirstValue(
                ClaimTypes.NameIdentifier)!);

        await _paymentMethodService.SetDefaultAsync(
            userId,
            id);

        return Ok(new
        {
            success = true,
            message = "Default payment method updated."
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        int id)
    {
        var userId =
            int.Parse(User.FindFirstValue(
                ClaimTypes.NameIdentifier)!);

        await _paymentMethodService.DeleteAsync(
            userId,
            id);

        return Ok(new
        {
            success = true,
            message = "Payment method deleted."
        });
    }
}