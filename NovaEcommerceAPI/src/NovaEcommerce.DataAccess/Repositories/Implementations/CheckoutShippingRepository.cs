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
    public class CheckoutShippingRepository : ICheckoutShippingRepository
    {
        private readonly AppDbContext _context;

        public CheckoutShippingRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(CheckoutSession checkout)
        {
            await _context.CheckoutSessions.AddAsync(checkout);
        }

        public async Task<CheckoutSession?> GetByIdAsync(int id)
        {
            return await _context.CheckoutSessions.FirstOrDefaultAsync(x=>x.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
