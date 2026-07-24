namespace NovaEcommerce.ServicesApp.DTOs.Users;

public class ChangePasswordDto
{
    public string CurrentPassword { get; set; } = default!;

    public string NewPassword { get; set; } = default!;
}