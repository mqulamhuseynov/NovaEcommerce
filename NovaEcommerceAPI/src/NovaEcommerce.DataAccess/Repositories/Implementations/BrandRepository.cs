using Microsoft.EntityFrameworkCore;
using NovaEcommerce.DataAccess.DbContext;
using NovaEcommerce.Domain.Entities;
using NovaEcommerce.ServicesApp.Services.Interfaces.Repository;

namespace NovaEcommerce.DataAccess.Repositories.Implementations;

public class BrandRepository : IBrandRepository
{
    private readonly AppDbContext _context;

    public BrandRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Brand?> GetBrandAsync(string slug)
    {
        return await _context.Brands
            .Include(x => x.Products)
                .ThenInclude(x => x.Images)
            .Include(x => x.Products)
                .ThenInclude(x => x.Variants)
            .Include(x => x.Products)
                .ThenInclude(x => x.Tags)
            .FirstOrDefaultAsync(x => x.Slug == slug);
    }
}