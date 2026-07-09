using Microsoft.EntityFrameworkCore;
using NovaEcommerce.DataAccess.DbContext;
using NovaEcommerce.Domain.Entities;
using NovaEcommerce.ServicesApp.Services.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.DataAccess.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Product?> GetProductDetails(int id)
        {
            return await _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Images)
                .Include(p => p.Reviews)
                   .ThenInclude(r => r.User) //istifadecinin adi lazim olar deye
                .Include(p => p.Variants)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
        }

        public IQueryable<Product> GetProductQuery()
        {
            return _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Include(p => p.Reviews)
                .Include(p => p.Variants)
                .Where(p => p.IsActive);
        }

        public async Task<IEnumerable<Product>> GetRelatedProducts(int categoryId, int currentProductId, int limit)
        {
            return await _context.Products
                .AsNoTracking()
                .Include(p => p.Images)
                .Where(p => p.CategoryId == categoryId && p.Id != currentProductId && p.IsActive)
                .Take(limit)
                .ToListAsync();
        }
    }
}
