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
    internal class BranchConfigration : IEntityTypeConfiguration<Branch>
    {
        public void Configure(EntityTypeBuilder<Branch> builder)
        {


            builder.Property(b => b.BranchName)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(b => b.BranchPhone)
                .IsRequired()
                .HasMaxLength(15);


            builder.Property(b => b.OpeningTime)
                .HasColumnType("time")
                .IsRequired();

            builder.Property(b => b.ClosingTime)
                .HasColumnType("time")
                .IsRequired();

            builder.Property(b => b.Is_Active)
                .HasDefaultValue(true)
                .IsRequired();

            builder.Property(b => b.ReviewCount)
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(b => b.AverageRating)
                .HasColumnType("decimal(18,1)")
                .HasDefaultValue(0m);

            builder.Property(b => b.Latitude)
                .IsRequired();

            builder.Property(b => b.Longitude)
               .IsRequired();
            builder.Property(b => b.Location).HasColumnType("geography");

            builder.Property(b => b.CreationDate).HasDefaultValueSql("GETDATE()");


            builder.HasMany(b => b.Categories)
                .WithOne(c => c.Branch)
                .HasForeignKey(c => c.BranchId)
                .OnDelete(DeleteBehavior.SetNull);


            builder.HasMany(b => b.Items)
                .WithOne(i => i.Branch)
                .HasForeignKey(i => i.BranchId)
                .OnDelete(DeleteBehavior.SetNull); 
                







        }
    }
}
