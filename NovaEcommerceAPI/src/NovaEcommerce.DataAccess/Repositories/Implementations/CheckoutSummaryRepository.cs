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
    public class CheckoutSummaryRepository : ICheckoutSummaryRepository
    {
        private readonly AppDbContext _context;

        public CheckoutSummaryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Cart?> GetCartWithItemsAsync(int userId)
        {
            return await _context.Carts
                            .Include(x=>x.Items)
                                .ThenInclude(x=>x.ProductVariant)
                                .ThenInclude(x=>x.Product)
                                .ThenInclude(x=>x.Images)
                         .FirstOrDefaultAsync(x=>x.UserId == userId);
        }

        public Task<CheckoutSession?> GetCheckoutSessionAsync(int checkoutSessionId)
        {
            return _context.CheckoutSessions
                            .Include(x=>x.PaymentMethod)
                            .FirstOrDefaultAsync(x=>x.Id == checkoutSessionId);
        }
    }
}
