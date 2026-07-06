using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovaEcommerce.DataAccess.Common;
using NovaEcommerce.Domain.Entities;

namespace NovaEcommerce.DataAccess.Configurations;

public class ReturnRequestConfiguration : IEntityTypeConfiguration<ReturnRequest>
{
    public void Configure(EntityTypeBuilder<ReturnRequest> builder)
    {
        builder.Property(r => r.Reason).HasMaxLength(500).IsRequired();
        builder.Property(r => r.ResolutionType).HasConversion<string>().HasMaxLength(20);
        builder.Property(r => r.ExchangeSize).HasMaxLength(20);
        builder.Property(r => r.Status).HasConversion<string>().HasMaxLength(20);

        builder.Property(r => r.Photos)
            .HasConversion(JsonListConverter.StringList)
            .Metadata.SetValueComparer(JsonListConverter.StringListComparer);

        // One return request per order item.
        builder.HasIndex(r => r.OrderItemId).IsUnique();

        builder.HasOne(r => r.OrderItem)
            .WithOne(oi => oi.ReturnRequest)
            .HasForeignKey<ReturnRequest>(r => r.OrderItemId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
