using NovaEcommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.Services.Interfaces.Repository
{
    public interface ICheckoutShippingRepository
    {
        Task AddAsync(CheckoutSession checkout);
        Task<CheckoutSession?> GetByIdAsync(int id);
        Task SaveChangesAsync();
    }
}
