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
    public class FlashSaleNotifyRepository : IFlashSaleNotifyRepository
    {
        private readonly AppDbContext _context;

        public FlashSaleNotifyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(NotifyRequest notify)
        {
            await _context.NotifyRequests.AddAsync(notify);
        }

        public async Task<bool> ExistsAsync(int flashSaleId, string email)
        {
            return await _context.NotifyRequests.AnyAsync(x=>x.FlashSaleId == flashSaleId && x.Email == email);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
