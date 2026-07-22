using Microsoft.AspNetCore.Http;

namespace NovaEcommerce.ServicesApp.DTOs.Users;

public class UploadAvatarDto
{
    public IFormFile Avatar { get; set; } = default!;
}