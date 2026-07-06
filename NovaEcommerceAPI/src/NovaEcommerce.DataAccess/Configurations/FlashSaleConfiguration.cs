using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovaEcommerce.Domain.Entities;

namespace NovaEcommerce.DataAccess.Configurations;

public class FlashSaleConfiguration : IEntityTypeConfiguration<FlashSale>
{
    public void Configure(EntityTypeBuilder<FlashSale> builder)
    {
        builder.Property(fs => fs.Name).HasMaxLength(150).IsRequired();

        builder.HasMany(fs => fs.Items)
            .WithOne(i => i.FlashSale)
            .HasForeignKey(i => i.FlashSaleId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
