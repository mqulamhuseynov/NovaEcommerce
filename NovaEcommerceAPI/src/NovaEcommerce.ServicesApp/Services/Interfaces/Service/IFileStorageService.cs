using Microsoft.AspNetCore.Http;

namespace NovaEcommerce.ServicesApp.Services.Interfaces.Service;

public interface IFileStorageService
{
    Task<string> SaveAvatarAsync(IFormFile file);
}