using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NovaEcommerce.DataAccess.DbContext;
using NovaEcommerce.Domain.Entities;
using NovaEcommerce.ServicesApp.Services.Interfaces.Repository;


namespace NovaEcommerce.DataAccess.Repositories.Implementations
{
    public class PlaceOrderRepository : IPlaceOrderRepository
    {
        private readonly AppDbContext _context;
        private IDbContextTransaction? _transaction;
        public PlaceOrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public async Task AddOrderItemAsync(List<OrderItem> orderItems)
        {
            await _context.OrderItems.AddRangeAsync(orderItems);
        }

        public async Task AddOrderStatusHistoryAsync(OrderStatusHistory history)
        {
            await _context.OrderStatusHistories.AddAsync(history);
        }

        public async Task<Cart?> GetCartByUserIdAsync(int userId)
        {
            return await _context.Carts
                            .Include(x => x.Items)
                                .ThenInclude(x => x.ProductVariant)
                                    .ThenInclude(x => x.Product)
                                        .ThenInclude(x => x.Images)
                            .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<CheckoutSession?> GetCheckoutSessionByUserIdAsync(int userId)
        {
            return await _context.CheckoutSessions
                            .Include(x => x.PaymentMethod)
                            .FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<bool> OrderNumberExistsAsync(string orderNumber)
        {
            return await _context.Orders.AnyAsync(x => x.OrderNumber == orderNumber);
        }

        public void RemoveCartItems(List<CartItem> cartItems)
        {
            _context.CartItems.RemoveRange(cartItems);
        }

        public void RemoveCheckoutSession(CheckoutSession checkoutSession)
        {
            _context.CheckoutSessions.Remove(checkoutSession);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void UpdateProductVariant(ProductVariant productVariant)
        {
            _context.ProductVariants.Update(productVariant);
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }

        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
            }
        }

        public async Task<Coupon?> GetCouponAsync(string code)
        {
            return await _context.Coupons
                             .FirstOrDefaultAsync(x =>
                             x.Code == code &&
                             x.IsActive &&
                             x.ExpiresAt > DateTime.UtcNow);
        }
    }
}
