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
    public class ReturnRequestRepository : IReturnRequestRepository
    {
        private readonly AppDbContext _context;
        public ReturnRequestRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(ReturnRequest request)
        {
            await _context.ReturnRequests.AddAsync(request);
        }

        public async Task<OrderItem?> GetOrderItemAsync(int orderItemId)
        {
            var orderItem = await _context.OrderItems
                                        .Include(x=>x.Order)
                                        .FirstOrDefaultAsync(x=>x.Id == orderItemId);
            return orderItem;
        }

        public async Task<List<ReturnRequest>> GetUserReturnRequestAsync(int userId)
        {
            var values = await _context.ReturnRequests
                                                .Include(x=>x.OrderItem)
                                                    .ThenInclude(x=>x.Order)
                                                .Where(x=>x.OrderItem.Order.UserId == userId)
                                                .OrderByDescending(x=>x.CreatedAt)
                                                .ToListAsync();
            return values;  
        }

        public async Task<ReturnRequest?> GetUserReturnRequestByIdAsync(int id, int userId)
        {
            var value = await _context.ReturnRequests
                                        .Include(x=>x.OrderItem)
                                            .ThenInclude(x=>x.Order)
                                        .FirstOrDefaultAsync(x=>x.Id==id && x.OrderItem.Order.UserId==userId);
            return value;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
