using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovaEcommerce.Domain.Entities;

namespace NovaEcommerce.DataAccess.Configurations;

public class FaqConfiguration : IEntityTypeConfiguration<Faq>
{
    public void Configure(EntityTypeBuilder<Faq> builder)
    {
        builder.Property(f => f.Category).HasMaxLength(100).IsRequired();
        builder.Property(f => f.Question).HasMaxLength(500).IsRequired();
        builder.Property(f => f.Answer).IsRequired();
        // Column named order_number in the spec; property is DisplayOrder to avoid
        // reading like "this FAQ has an order number" (confusable with Order.OrderNumber).
        builder.Property(f => f.DisplayOrder).HasColumnName("order_number");
    }
}
