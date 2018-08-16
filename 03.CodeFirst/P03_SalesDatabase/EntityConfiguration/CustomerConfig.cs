using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P03_SalesDatabase.Data.Models;

namespace P03_SalesDatabase.EntityConfiguration
{
    public class CustomerConfig : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasKey(c => c.CustomerId);

            builder.Property(c => c.Name)
                .HasMaxLength(80)
                .IsUnicode()
                .IsRequired();

            builder.Property(c => c.Email)
                .HasMaxLength(100)
                .IsUnicode()
                .IsRequired();

            builder.Property(c => c.CreditCardNumber)
                 .IsRequired();
        }
    }
}
