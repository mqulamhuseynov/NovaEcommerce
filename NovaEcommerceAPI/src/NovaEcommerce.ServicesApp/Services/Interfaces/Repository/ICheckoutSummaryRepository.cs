using NovaEcommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.Services.Interfaces.Repository
{
    public interface ICheckoutSummaryRepository
    {
        Task<CheckoutSession?> GetCheckoutSessionAsync(int checkoutSessionId);

        Task<Cart?> GetCartWithItemsAsync(int userId);
    }
}
