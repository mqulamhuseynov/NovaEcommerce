using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovaEcommerce.Domain.Entities;

namespace NovaEcommerce.DataAccess.Configurations;

public class PaymentMethodConfiguration : IEntityTypeConfiguration<PaymentMethod>
{
    public void Configure(EntityTypeBuilder<PaymentMethod> builder)
    {
        builder.Property(p => p.CardType).HasConversion<string>().HasMaxLength(20);
        builder.Property(p => p.LastFourDigits).HasMaxLength(4).IsRequired();
        builder.Property(p => p.CardholderName).HasMaxLength(150).IsRequired();
    }
}
