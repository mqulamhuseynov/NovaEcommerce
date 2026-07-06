using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovaEcommerce.Domain.Entities;

namespace NovaEcommerce.DataAccess.Configurations;

public class SupportArticleConfiguration : IEntityTypeConfiguration<SupportArticle>
{
    public void Configure(EntityTypeBuilder<SupportArticle> builder)
    {
        builder.Property(a => a.Title).HasMaxLength(300).IsRequired();
        builder.Property(a => a.Content).IsRequired();
        builder.Property(a => a.Category).HasMaxLength(100).IsRequired();
    }
}
