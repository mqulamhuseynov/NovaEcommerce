using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NovaEcommerce.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NovaEcommerce.DataAccess.Configurations
{
    public class CheckoutSessionConfiguration : IEntityTypeConfiguration<CheckoutSession>   
    {
        public void Configure(
       EntityTypeBuilder<CheckoutSession> builder)
        {

            builder.Property(x => x.Email)
                .HasMaxLength(150)
                .IsRequired();


            builder.Property(x => x.ShippingMethod)
                .HasConversion<string>()
                .HasMaxLength(20);


            builder.Property(x => x.ShippingCost)
                .HasColumnType("decimal(18,2)");


            builder.HasOne(x => x.User)
                .WithMany()
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
