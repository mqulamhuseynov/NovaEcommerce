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
    public class CheckoutPaymentRepository : ICheckoutPaymentRepository
    {
        private readonly AppDbContext _context;

        public CheckoutPaymentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddPaymentMethodAsync(PaymentMethod payment)
        {
            await _context.PaymentMethods.AddAsync(payment);
        }

        public async Task<CheckoutSession?> GetCheckoutSessionAsync(int id)
        {
            return await _context.CheckoutSessions.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<PaymentMethod>> GetUserPaymentMethodsAsync(int? userId)
        {
            return await _context.PaymentMethods
                                    .Where(x=>x.UserId == userId)
                                    .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
