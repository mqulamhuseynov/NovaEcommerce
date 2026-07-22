using NovaEcommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.Services.Interfaces.Repository
{
    public interface IReturnRequestRepository
    {
        Task AddAsync(ReturnRequest request);

        Task<List<ReturnRequest>> GetUserReturnRequestAsync(int userId);

        Task<ReturnRequest?> GetUserReturnRequestByIdAsync(int id, int userId);

        Task<OrderItem?> GetOrderItemAsync(int orderItemId);

        Task SaveChangesAsync();
    }
}
