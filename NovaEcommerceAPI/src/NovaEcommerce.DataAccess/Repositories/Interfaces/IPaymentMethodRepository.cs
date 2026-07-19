using NovaEcommerce.Domain.Entities;

namespace NovaEcommerce.DataAccess.Repositories.Interfaces;

public interface IPaymentMethodRepository
{
    Task<List<PaymentMethod>> GetAllAsync(int userId);

    Task<PaymentMethod?> GetByIdAsync(int id, int userId);

    Task AddAsync(PaymentMethod paymentMethod);

    Task UpdateAsync(PaymentMethod paymentMethod);

    Task DeleteAsync(PaymentMethod paymentMethod);

    Task SaveChangesAsync();
}