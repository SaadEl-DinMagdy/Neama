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
    internal class AppUserConfigration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(u => u.DisplayName)
            .IsRequired()
            .HasMaxLength(50);

            builder.Property(u => u.MealsSaved)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(u => u.SavedMoney)
                .IsRequired()
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0m);

            
            builder.HasMany(u => u.Addresses)
                .WithOne(a => a.AppUser)
                .HasForeignKey(a => a.AppUserId)
                .OnDelete(DeleteBehavior.SetNull);


            builder.HasOne(u => u.Partner)
                .WithOne(p => p.Manager)
                .HasForeignKey<Partner>(p => p.ManagerId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(u => u.Charity)
                .WithOne(c => c.Manager)
                .HasForeignKey<Charity>(c => c.ManagerId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(u => u.Branch)
                .WithOne(b => b.Manager)
                .HasForeignKey<Branch>(b => b.ManagerId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
