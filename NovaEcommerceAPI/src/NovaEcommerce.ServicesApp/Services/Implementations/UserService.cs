using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NovaEcommerce.Domain.Entities;
using NovaEcommerce.ServicesApp.DTOs.Responses;
using NovaEcommerce.ServicesApp.DTOs.Users;
using NovaEcommerce.ServicesApp.Services.Interfaces.Service;

namespace NovaEcommerce.ServicesApp.Services.Implementations;

public class UserService : IUserService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IFileStorageService _fileStorageService;

    public UserService(
        UserManager<AppUser> userManager,
        IFileStorageService fileStorageService)
    {
        _userManager = userManager;
        _fileStorageService = fileStorageService;
    }

    public async Task<ApiResponse<string>> UpdateProfileAsync(
        int userId,
        UpdateProfileDto dto)
    {
        var user = await _userManager.Users
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
        {
            return ApiResponse<string>.FailResponse(
                "User not found.",
                404);
        }

        if (!string.Equals(user.Email, dto.Email, StringComparison.OrdinalIgnoreCase))
        {
            var emailExists = await _userManager.Users
     .AnyAsync(x =>
         x.NormalizedEmail == dto.Email.ToUpper()
         && x.Id != userId);

            if (emailExists)
            {
                return ApiResponse<string>.FailResponse(
                    "Email already exists.",
                    400);
            }

            user.Email = dto.Email;
            user.UserName = dto.Email;
            user.NormalizedEmail = dto.Email.ToUpper();
            user.NormalizedUserName = dto.Email.ToUpper();
        }

        user.FirstName = dto.FirstName;
        user.LastName = dto.LastName;
        user.PhoneNumber = dto.Phone;
        user.UpdatedAt = DateTime.UtcNow;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return ApiResponse<string>.FailResponse(
                string.Join(", ", result.Errors.Select(x => x.Description)),
                400);
        }

        return ApiResponse<string>.SuccessResponse(
            "Profile updated successfully.");
    }
    public async Task<ApiResponse<string>> ChangePasswordAsync(
    int userId,
    ChangePasswordDto dto)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            return ApiResponse<string>.FailResponse(
                "User not found.",
                404);
        }

        var result = await _userManager.ChangePasswordAsync(
            user,
            dto.CurrentPassword,
            dto.NewPassword);

        if (!result.Succeeded)
        {
            return ApiResponse<string>.FailResponse(
                string.Join(", ", result.Errors.Select(x => x.Description)),
                400);
        }

        user.UpdatedAt = DateTime.UtcNow;

        await _userManager.UpdateAsync(user);

        return ApiResponse<string>.SuccessResponse(
            "Password changed successfully.");
    }

    public async Task<ApiResponse<string>> UploadAvatarAsync(
        int userId,
        UploadAvatarDto dto)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user == null)
        {
            return ApiResponse<string>.FailResponse(
                "User not found.",
                404);
        }

        if (dto.Avatar == null || dto.Avatar.Length == 0)
        {
            return ApiResponse<string>.FailResponse(
                "Avatar is required.",
                400);
        }

        var avatarUrl = await _fileStorageService.SaveAvatarAsync(dto.Avatar);

        user.AvatarUrl = avatarUrl;
        user.UpdatedAt = DateTime.UtcNow;

        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            return ApiResponse<string>.FailResponse(
                string.Join(", ", result.Errors.Select(x => x.Description)),
                400);
        }

        return ApiResponse<string>.SuccessResponse(
            "Avatar uploaded successfully.");
    }
}