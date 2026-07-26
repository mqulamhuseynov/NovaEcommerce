using Microsoft.EntityFrameworkCore;
using NovaEcommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.Services.Interfaces.Repository
{
    public interface ICheckoutPaymentRepository
    {
        Task AddPaymentMethodAsync(PaymentMethod payment);
        Task<CheckoutSession?> GetCheckoutSessionAsync(int id);
        Task<bool> HasPaymentMethodsAsync(int userId);
        Task SaveChangesAsync();    
    }
}
