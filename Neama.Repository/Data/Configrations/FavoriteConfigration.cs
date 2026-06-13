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
    internal class FavoriteConfigration : IEntityTypeConfiguration<Favorite>
    {
        public void Configure(EntityTypeBuilder<Favorite> builder)
        {
            builder.HasIndex(f => new { f.UserId, f.BranchId })
                   .IsUnique();

            builder.HasOne(f => f.User)
                .WithMany()
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.Branch)
                .WithMany()
                .HasForeignKey(f => f.BranchId)
                .OnDelete(DeleteBehavior.Cascade);

        }
    }
}
