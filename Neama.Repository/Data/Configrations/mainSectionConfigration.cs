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
    internal class mainSectionConfigration : IEntityTypeConfiguration<MainSection>
    {
        public void Configure(EntityTypeBuilder<MainSection> builder)
        {
            builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(100);

            builder.Property(m => m.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(m => m.IconURL).IsRequired(false);

            builder.Property(m => m.CreationDate).HasDefaultValueSql("GETDATE()");
        }
    }
}
