using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NovaEcommerce.Domain.Entities;

namespace NovaEcommerce.DataAccess.DbContext;

public class AppDbContext : IdentityDbContext<AppUser, IdentityRole<int>, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();
    public DbSet<ProductImage> ProductImages => Set<ProductImage>();
    public DbSet<ProductTag> ProductTags => Set<ProductTag>();
    public DbSet<FlashSale> FlashSales => Set<FlashSale>();
    public DbSet<FlashSaleItem> FlashSaleItems => Set<FlashSaleItem>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Coupon> Coupons => Set<Coupon>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<PaymentMethod> PaymentMethods => Set<PaymentMethod>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<OrderStatusHistory> OrderStatusHistories => Set<OrderStatusHistory>();
    public DbSet<Wishlist> Wishlists => Set<Wishlist>();
    public DbSet<WishlistItem> WishlistItems => Set<WishlistItem>();
    public DbSet<ReturnRequest> ReturnRequests => Set<ReturnRequest>();
    public DbSet<Faq> Faqs => Set<Faq>();
    public DbSet<SupportArticle> SupportArticles => Set<SupportArticle>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<HeroBanner> HeroBanners => Set<HeroBanner>();
    public DbSet<NotifyRequest> NotifyRequests => Set<NotifyRequest>(); 

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // Identity's own model setup (AspNetUsers/AspNetRoles/etc.) runs first...
        base.OnModelCreating(builder);
        // ...then our configs run, so overrides like AppUserConfiguration's ToTable("users")
        // win over Identity's defaults.
        builder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

    }
}
