namespace NovaEcommerce.ServicesApp.DTOs;

public class AuthResponseDto
{
    public string AccessToken { get; set; } = default!;

    public string RefreshToken { get; set; } = default!;

    public UserDto User { get; set; } = default!;
}