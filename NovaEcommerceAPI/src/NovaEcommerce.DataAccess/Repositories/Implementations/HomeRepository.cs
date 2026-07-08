using Microsoft.EntityFrameworkCore;
using NovaEcommerce.DataAccess.DbContext;
using NovaEcommerce.Domain.Entities;
using NovaEcommerce.Domain.Enums;
using NovaEcommerce.ServicesApp.Services.Interfaces.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.DataAccess.Repositories.Implementations
{
    public class HomeRepository : IHomeRepository
    {
        private readonly AppDbContext _context;

        public HomeRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            var result = await _context.Categories
                .Where(x=>x.ParentCategoryId == null)
                .OrderBy(x=>x.Name)
                .ToListAsync();
            return result;
        }

        public async Task<List<Product>> GetCuratedPicksAsync()
        {
            var result = await _context.Products
                .Include(x=>x.Brand)
                .Include(x=>x.Images)
                .Include(x=>x.Tags)
                .Include(x=>x.Variants)
                .Where(x=>x.IsActive && x.Tags.Any(t=>t.TagType == TagType.Featured))
                .ToListAsync();
            return result;
        }

        public async Task<List<HeroBanner>> GetHeroBannersAsync()
        {
            var result = await _context.HeroBanners
                .Where(x=>x.IsActive)
                .OrderBy(x=>x.DisplayOrder)
                .ToListAsync();
            return result;
        }
    }
}
