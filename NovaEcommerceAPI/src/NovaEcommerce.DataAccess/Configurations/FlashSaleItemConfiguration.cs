using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovaEcommerce.Domain.Entities;

namespace NovaEcommerce.DataAccess.Configurations;

public class FlashSaleItemConfiguration : IEntityTypeConfiguration<FlashSaleItem>
{
    public void Configure(EntityTypeBuilder<FlashSaleItem> builder)
    {
        builder.Property(i => i.DiscountPercentage).HasPrecision(5, 2);
        builder.HasIndex(i => new { i.FlashSaleId, i.ProductVariantId }).IsUnique();
    }
}
