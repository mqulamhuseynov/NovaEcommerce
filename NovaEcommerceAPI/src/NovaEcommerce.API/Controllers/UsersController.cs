using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NovaEcommerce.ServicesApp.DTOs.Users;
using NovaEcommerce.ServicesApp.Services.Interfaces.Service;
using System.Security.Claims;

namespace NovaEcommerce.API.Controllers;

[Route("api/users")]
[ApiController]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _service;

    public UsersController(IUserService service)
    {
        _service = service;
    }

    [HttpPatch("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var result = await _service.UpdateProfileAsync(userId, dto);

        return result.StatusCode switch
        {
            200 => Ok(result),
            400 => BadRequest(result),
            404 => NotFound(result),
            _ => StatusCode(result.StatusCode, result)
        };
    }

    [HttpPatch("password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var result = await _service.ChangePasswordAsync(userId, dto);

        return result.StatusCode switch
        {
            200 => Ok(result),
            400 => BadRequest(result),
            404 => NotFound(result),
            _ => StatusCode(result.StatusCode, result)
        };
    }

    [HttpPost("avatar")]
    public async Task<IActionResult> UploadAvatar([FromForm] UploadAvatarDto dto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

        var result = await _service.UploadAvatarAsync(userId, dto);

        return result.StatusCode switch
        {
            200 => Ok(result),
            400 => BadRequest(result),
            404 => NotFound(result),
            _ => StatusCode(result.StatusCode, result)
        };
    }
}