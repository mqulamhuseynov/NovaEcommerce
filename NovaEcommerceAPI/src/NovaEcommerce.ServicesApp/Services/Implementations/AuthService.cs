using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NovaEcommerce.DataAccess.DbContext;
using NovaEcommerce.Domain.Entities;
using NovaEcommerce.ServicesApp.DTOs;
using NovaEcommerce.ServicesApp.Services.Interfaces;

namespace NovaEcommerce.ServicesApp.Services.Implementations;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly AppDbContext _context;
    private readonly IJwtService _jwtService;

    public AuthService(
        UserManager<AppUser> userManager,
        AppDbContext context,
        IJwtService jwtService)
    {
        _userManager = userManager;
        _context = context;
        _jwtService = jwtService;
    }

    private static UserDto MapUser(AppUser user)
    {
        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email!
        };
    }

    private async Task SaveRefreshTokenAsync(AppUser user, string refreshToken)
    {
        var days = int.Parse(
            Environment.GetEnvironmentVariable("REFRESH_TOKEN_EXPIRES_DAYS") ?? "7");

        var token = new RefreshToken
        {
            UserId = user.Id,
            Token = refreshToken,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(days)
        };

        _context.RefreshTokens.Add(token);

        await _context.SaveChangesAsync();
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
    {
        var existingUser =
            await _userManager.FindByEmailAsync(request.Email);

        if (existingUser != null)
            throw new Exception("Email already exists.");

        var user = new AppUser
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            UserName = request.Email,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            IsGuest = false
        };

        var result =
            await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            throw new Exception(
                string.Join(", ", result.Errors.Select(x => x.Description)));
        }

        var accessToken =
            _jwtService.GenerateAccessToken(user);

        var refreshToken =
            _jwtService.GenerateRefreshToken();

        await SaveRefreshTokenAsync(user, refreshToken);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            User = MapUser(user)
        };
    }

    public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
    {
        var user =
            await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
            throw new Exception("Invalid email or password.");

        var passwordCorrect =
            await _userManager.CheckPasswordAsync(user, request.Password);

        if (!passwordCorrect)
            throw new Exception("Invalid email or password.");

        var accessToken =
            _jwtService.GenerateAccessToken(user);

        var refreshToken =
            _jwtService.GenerateRefreshToken();

        await SaveRefreshTokenAsync(user, refreshToken);

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            User = MapUser(user)
        };
    }
    public async Task<AuthResponseDto> RefreshTokenAsync(string refreshToken)
    {
        var token =
            await _context.RefreshTokens
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Token == refreshToken);

        if (token == null)
            throw new Exception("Invalid refresh token.");

        if (token.ExpiresAt < DateTime.UtcNow)
        {
            _context.RefreshTokens.Remove(token);
            await _context.SaveChangesAsync();

            throw new Exception("Refresh token expired.");
        }

        var user = token.User;

        var newAccessToken =
            _jwtService.GenerateAccessToken(user);

        var newRefreshToken =
            _jwtService.GenerateRefreshToken();

        _context.RefreshTokens.Remove(token);

        await SaveRefreshTokenAsync(user, newRefreshToken);

        return new AuthResponseDto
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            User = MapUser(user)
        };
    }

    public async Task LogoutAsync(string refreshToken)
    {
        var token =
            await _context.RefreshTokens
                .FirstOrDefaultAsync(x => x.Token == refreshToken);

        if (token == null)
            return;

        _context.RefreshTokens.Remove(token);

        await _context.SaveChangesAsync();
    }

    public async Task<UserDto> GetMeAsync(int userId)
    {
        var user =
            await _userManager.Users
                .FirstOrDefaultAsync(x => x.Id == userId);

        if (user == null)
            throw new Exception("User not found.");

        return new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email!
        };
    }
}
