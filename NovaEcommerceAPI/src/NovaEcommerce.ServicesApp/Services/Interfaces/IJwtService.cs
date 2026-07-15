using NovaEcommerce.Domain.Entities;
using System.Security.Claims;

namespace NovaEcommerce.ServicesApp.Services.Interfaces;

public interface IJwtService
{
    string GenerateAccessToken(AppUser user);

    string GenerateRefreshToken();

    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}