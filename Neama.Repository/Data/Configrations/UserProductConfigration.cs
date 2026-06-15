using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neama.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Repository.Data.Configrations
{
    internal class UserProductConfigration : IEntityTypeConfiguration<UserProduct>
    {
        public void Configure(EntityTypeBuilder<UserProduct> builder)
        {


            builder.Property(up => up.Name)
                .IsRequired()
                .HasMaxLength(150); 

            builder.Property(up => up.Discription) 
                .HasMaxLength(1000);

            builder.Property(up => up.Country)
                .HasMaxLength(50);

            builder.Property(up => up.City)
                .HasMaxLength(50);

            builder.Property(up => up.Phone)
                .HasMaxLength(20);

            builder.Property(up => up.WhatsApp)
                .HasMaxLength(20);


             builder.Property(up => up.Price)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(up=> up.CreationDate).HasDefaultValueSql("GETDATE()");


            builder.HasOne(up => up.User)
                .WithMany(u => u.UserProduct) 
                .HasForeignKey(up => up.AppUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
