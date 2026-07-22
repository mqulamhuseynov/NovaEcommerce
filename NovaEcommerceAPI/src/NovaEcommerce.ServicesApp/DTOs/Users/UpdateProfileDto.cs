namespace NovaEcommerce.ServicesApp.DTOs.Users;

public class UpdateProfileDto
{
    public string FirstName { get; set; } = default!;

    public string LastName { get; set; } = default!;

    public string Email { get; set; } = default!;

    public string Phone { get; set; } = default!;
}