using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P03_SalesDatabase.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P03_SalesDatabase.EntityConfiguration
{
    public class SaleConfig : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> builder)
        {
            builder.HasKey(s => s.SaleId);

            builder.Property(s => s.Date)
                .IsRequired(true)
                .HasDefaultValueSql("GETDATE()");

            builder.HasOne(x => x.Product)
                .WithMany(p => p.Sales)
                .HasForeignKey(x => x.ProductId);

            builder.HasOne(x => x.Customer)
                .WithMany(p => p.Sales)
                .HasForeignKey(x => x.CustomerId);

            builder.HasOne(x => x.Store)
                .WithMany(p => p.Sales)
                .HasForeignKey(x => x.StoreId);
        }
    }
}
