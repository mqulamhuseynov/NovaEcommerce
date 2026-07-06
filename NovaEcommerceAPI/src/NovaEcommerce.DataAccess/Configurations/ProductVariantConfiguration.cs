using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovaEcommerce.Domain.Entities;

namespace NovaEcommerce.DataAccess.Configurations;

public class ProductVariantConfiguration : IEntityTypeConfiguration<ProductVariant>
{
    public void Configure(EntityTypeBuilder<ProductVariant> builder)
    {
        builder.Property(v => v.Color).HasMaxLength(50).IsRequired();
        builder.Property(v => v.Size).HasMaxLength(20).IsRequired();
        builder.Property(v => v.Sku).HasMaxLength(50).IsRequired();
        builder.Property(v => v.Price).HasPrecision(18, 2);
        builder.Property(v => v.OriginalPrice).HasPrecision(18, 2);

        builder.HasIndex(v => v.Sku).IsUnique();

        // This is the constraint that actually enforces "a product has many colors,
        // each color has many sizes, each with its own stock": no duplicate
        // (Product, Color, Size) rows.
        builder.HasIndex(v => new { v.ProductId, v.Color, v.Size }).IsUnique();

        builder.HasMany(v => v.CartItems)
            .WithOne(ci => ci.ProductVariant)
            .HasForeignKey(ci => ci.ProductVariantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(v => v.OrderItems)
            .WithOne(oi => oi.ProductVariant)
            .HasForeignKey(oi => oi.ProductVariantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(v => v.WishlistItems)
            .WithOne(wi => wi.ProductVariant)
            .HasForeignKey(wi => wi.ProductVariantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(v => v.FlashSaleItems)
            .WithOne(fsi => fsi.ProductVariant)
            .HasForeignKey(fsi => fsi.ProductVariantId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
