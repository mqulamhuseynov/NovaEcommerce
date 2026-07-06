using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovaEcommerce.DataAccess.Common;
using NovaEcommerce.Domain.Entities;

namespace NovaEcommerce.DataAccess.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.Property(r => r.Title).HasMaxLength(200);
        builder.Property(r => r.FitRating).HasConversion<string>().HasMaxLength(20);

        builder.Property(r => r.Photos)
            .HasConversion(JsonListConverter.StringList)
            .Metadata.SetValueComparer(JsonListConverter.StringListComparer);

        // One review per purchased item — enforces the "verified purchase" model.
        builder.HasIndex(r => r.OrderItemId).IsUnique();

        builder.HasOne(r => r.OrderItem)
            .WithOne(oi => oi.Review)
            .HasForeignKey<Review>(r => r.OrderItemId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
