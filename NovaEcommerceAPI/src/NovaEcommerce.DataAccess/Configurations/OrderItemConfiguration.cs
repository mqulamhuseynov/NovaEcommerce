using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovaEcommerce.Domain.Entities;

namespace NovaEcommerce.DataAccess.Configurations;

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.Property(oi => oi.ProductNameSnapshot).HasMaxLength(200).IsRequired();
        builder.Property(oi => oi.ColorSnapshot).HasMaxLength(50).IsRequired();
        builder.Property(oi => oi.SizeSnapshot).HasMaxLength(20).IsRequired();
        builder.Property(oi => oi.PriceSnapshot).HasPrecision(18, 2);
    }
}
