using NovaEcommerce.Domain.Entities;

namespace NovaEcommerce.DataAccess.Repositories.Interfaces;

public interface IBrandRepository
{
    Task<Brand?> GetBrandAsync(string slug);
}