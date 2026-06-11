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
    internal class ApplicationToJoinConfiguration : IEntityTypeConfiguration<ApplicationsToJoin>
    {
        public void Configure(EntityTypeBuilder<ApplicationsToJoin> builder)
        {

            builder.Property(a => a.FullName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(a => a.PlaceName)
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(a => a.CreatedAt)
                .HasDefaultValueSql("GETDATE()");

            builder.Property(a => a.ContactWasMade)
                .HasDefaultValue(false);
        }
    }
}
