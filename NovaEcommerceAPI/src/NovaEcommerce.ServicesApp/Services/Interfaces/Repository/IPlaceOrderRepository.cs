using NovaEcommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.ServicesApp.Services.Interfaces.Repository
{
    public interface IPlaceOrderRepository
    {
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();    
        Task<CheckoutSession?> GetCheckoutSessionByUserIdAsync(int userId);   

        Task<Cart?> GetCartByUserIdAsync(int userId);

        Task AddOrderAsync(Order order); 

        Task AddOrderItemAsync(List<OrderItem> orderItems);

        Task AddOrderStatusHistoryAsync(OrderStatusHistory history);

        void UpdateProductVariant(ProductVariant productVariant);

        void RemoveCartItems(List<CartItem> cartItems);

        Task<bool> OrderNumberExistsAsync(string orderNumber);

        void RemoveCheckoutSession(CheckoutSession checkoutSession);

        Task SaveChangesAsync(); 
    }
}
