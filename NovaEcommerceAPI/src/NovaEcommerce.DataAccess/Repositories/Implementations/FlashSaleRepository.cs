using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens.Experimental;
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
    public class FlashSaleRepository : IFlashSaleRepository
    {
        private readonly AppDbContext _context;

        public FlashSaleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<FlashSale?> GetActiveAsync()
        {
            var data = await _context.FlashSales
                            .AsNoTracking()
                            .Include(x=>x.Items)
                            .ThenInclude(x=>x.ProductVariant)
                            .ThenInclude(x=>x.Product)
                            .ThenInclude(x => x.Images)
                            .Where(x=> 
                                        x.IsActive && x.StartsAt<=DateTime.UtcNow && x.EndsAt>DateTime.UtcNow)
                            .FirstOrDefaultAsync();
            return data;
        }

        public async Task<FlashSale?> GetByIdAsync(int id)
        {
            var data = await _context.FlashSales
                            .Include(x=>x.Items)
                            .FirstOrDefaultAsync(x=>x.Id == id);
            return data;
        }

        public async Task<List<FlashSale>> GetUpComingAsync()
        {
            var data = await _context.FlashSales.AsNoTracking()
                            .Include(x=>x.Items)
                            .ThenInclude(x=>x.ProductVariant)
                            .ThenInclude(x=>x.Product)
                            .ThenInclude(x => x.Images)
                            .Where(x=>x.StartsAt>DateTime.UtcNow)
                            .OrderBy(x=>x.StartsAt)
                            .ToListAsync();
            return data;
        }
    }
}
