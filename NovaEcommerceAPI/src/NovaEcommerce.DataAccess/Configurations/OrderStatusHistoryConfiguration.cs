using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovaEcommerce.Domain.Entities;

namespace NovaEcommerce.DataAccess.Configurations;

public class OrderStatusHistoryConfiguration : IEntityTypeConfiguration<OrderStatusHistory>
{
    public void Configure(EntityTypeBuilder<OrderStatusHistory> builder)
    {
        // Explicit override: naming-convention pluralization of the DbSet name would
        // otherwise produce "order_status_histories", but the spec wants the singular
        // "order_status_history".
        builder.ToTable("order_status_history");
        builder.Property(h => h.Status).HasConversion<string>().HasMaxLength(20);
    }
}
