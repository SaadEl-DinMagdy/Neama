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
    public class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Comment).IsRequired().HasMaxLength(1000);
            builder.Property(r => r.Rating).IsRequired();
            builder.Property(r => r.ImageUrl).IsRequired(); 
            builder.Property(r => r.CreationDate).IsRequired();

            builder.HasOne(r => r.Branch)
                .WithMany()
                .HasForeignKey(r => r.BranchId);

            builder.HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId);
               
        }
    }
}
