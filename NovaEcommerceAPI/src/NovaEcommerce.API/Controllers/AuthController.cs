using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NovaEcommerce.ServicesApp.DTOs;
using NovaEcommerce.ServicesApp.Services.Interfaces;
using System.Security.Claims;

namespace NovaEcommerce.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDto request)
    {
        var result = await _authService.RegisterAsync(request);

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDto request)
    {
        var result = await _authService.LoginAsync(request);

        return Ok(result);
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken(
        RefreshTokenRequestDto request)
    {
        var result =
            await _authService.RefreshTokenAsync(request.RefreshToken);

        return Ok(result);
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout(
        LogoutRequestDto request)
    {
        await _authService.LogoutAsync(request.RefreshToken);

        return Ok(new
        {
            message = "Logged out successfully"
        });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> Me()
    {
        var userId =
            int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result =
            await _authService.GetMeAsync(userId);

        return Ok(result);
    }
}