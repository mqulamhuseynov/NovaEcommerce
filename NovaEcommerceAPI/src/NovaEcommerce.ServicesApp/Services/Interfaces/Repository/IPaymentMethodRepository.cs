using NovaEcommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.Services.Interfaces.Repository
{
    public interface IPaymentMethodRepository
    {
        Task<List<PaymentMethod>> GetAllAsync(int userId);

        Task<PaymentMethod?> GetByIdAsync(int id, int userId);

        Task AddAsync(PaymentMethod paymentMethod);

        Task UpdateAsync(PaymentMethod paymentMethod);

        Task DeleteAsync(PaymentMethod paymentMethod);

        Task SaveChangesAsync();
    }
}
