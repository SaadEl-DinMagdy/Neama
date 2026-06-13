using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neama.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Repository.Data.Configrations
{
    internal class ItemConfigration : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.Property(i => i.Name)
            .IsRequired()
            .HasMaxLength(150);

            builder.Property(i => i.Description)
                .IsRequired()
                .HasMaxLength(2500); 

            builder.Property(i => i.OriginalPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(i => i.DiscountPrice)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(i => i.StockQuantity)
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(i => i.IsActive)
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(i => i.ImageURL)
                .IsRequired(false);

            builder.Property(i => i.ExpiryDate)
                .IsRequired(false);

            builder.Property(i => i.CreationDate).HasDefaultValueSql("GETDATE()");

            builder.HasOne(i => i.Category)
                .WithMany(c => c.Items)
                .HasForeignKey(i => i.CategoryId)
                .IsRequired(false) 
                .OnDelete(DeleteBehavior.SetNull); 

          
        }
    }
}
