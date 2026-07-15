using Microsoft.EntityFrameworkCore;
using NovaEcommerce.DataAccess.DbContext;
using NovaEcommerce.DataAccess.Repositories.Interfaces;
using NovaEcommerce.Domain.Entities;

namespace NovaEcommerce.DataAccess.Repositories.Implementations;

public class PaymentMethodRepository : IPaymentMethodRepository
{
    private readonly AppDbContext _context;

    public PaymentMethodRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<PaymentMethod>> GetAllAsync(int userId)
    {
        return await _context.PaymentMethods
            .Where(x => x.UserId == userId)
            .ToListAsync();
    }

    public async Task<PaymentMethod?> GetByIdAsync(int id, int userId)
    {
        return await _context.PaymentMethods
            .FirstOrDefaultAsync(x =>
                x.Id == id &&
                x.UserId == userId);
    }

    public async Task AddAsync(PaymentMethod paymentMethod)
    {
        await _context.PaymentMethods.AddAsync(paymentMethod);
    }

    public Task UpdateAsync(PaymentMethod paymentMethod)
    {
        _context.PaymentMethods.Update(paymentMethod);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(PaymentMethod paymentMethod)
    {
        _context.PaymentMethods.Remove(paymentMethod);

        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}