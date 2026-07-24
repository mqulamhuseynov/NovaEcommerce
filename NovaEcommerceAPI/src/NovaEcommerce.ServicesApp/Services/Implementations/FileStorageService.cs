using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using NovaEcommerce.ServicesApp.Services.Interfaces.Service;

namespace NovaEcommerce.ServicesApp.Services.Implementations;

public class FileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _environment;

    public FileStorageService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> SaveAvatarAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new Exception("Avatar file is required.");

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        var allowedExtensions = new[]
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".webp"
        };

        if (Array.IndexOf(allowedExtensions, extension) < 0)
            throw new Exception("Only jpg, jpeg, png and webp files are allowed.");

        if (file.Length > 5 * 1024 * 1024)
            throw new Exception("Maximum file size is 5 MB.");

        var webRoot = _environment.WebRootPath;

        if (string.IsNullOrEmpty(webRoot))
        {
            webRoot = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot");
        }

        var folder = Path.Combine(webRoot, "uploads", "avatars");

        if (!Directory.Exists(folder))
        {
            Directory.CreateDirectory(folder);
        }

        var fileName = $"{Guid.NewGuid()}{extension}";

        var filePath = Path.Combine(folder, fileName);

        using var stream = new FileStream(filePath, FileMode.Create);

        await file.CopyToAsync(stream);

        return $"/uploads/avatars/{fileName}";
    }
}