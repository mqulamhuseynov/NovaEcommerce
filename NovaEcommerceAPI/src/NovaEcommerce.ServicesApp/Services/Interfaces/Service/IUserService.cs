using NovaEcommerce.ServicesApp.DTOs.Responses;
using NovaEcommerce.ServicesApp.DTOs.Users;

namespace NovaEcommerce.ServicesApp.Services.Interfaces.Service;

public interface IUserService
{
    Task<ApiResponse<string>> UpdateProfileAsync(
        int userId,
        UpdateProfileDto dto);

    Task<ApiResponse<string>> ChangePasswordAsync(
        int userId,
        ChangePasswordDto dto);

    Task<ApiResponse<string>> UploadAvatarAsync(
        int userId,
        UploadAvatarDto dto);
}