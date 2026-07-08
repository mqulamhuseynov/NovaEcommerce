using NovaEcommerce.ServicesApp.DTOs;

namespace NovaEcommerce.ServicesApp.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);

    Task<AuthResponseDto> LoginAsync(LoginRequestDto request);

    Task<AuthResponseDto> RefreshTokenAsync(string refreshToken);

    Task LogoutAsync(string refreshToken);

    Task<UserDto> GetMeAsync(int userId);
}