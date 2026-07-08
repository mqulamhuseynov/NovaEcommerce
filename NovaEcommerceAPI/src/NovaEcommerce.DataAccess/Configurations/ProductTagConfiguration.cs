using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovaEcommerce.Domain.Entities;

namespace NovaEcommerce.DataAccess.Configurations;

public class ProductTagConfiguration : IEntityTypeConfiguration<ProductTag>
{
    public void Configure(EntityTypeBuilder<ProductTag> builder)
    {

        builder.Property(t => t.TagType).HasConversion<string>().HasMaxLength(20);
        builder.Property(t => t.Value).HasMaxLength(100);
    }
}
