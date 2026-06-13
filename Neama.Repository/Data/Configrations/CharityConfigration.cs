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
    internal class CharityConfigration : IEntityTypeConfiguration<Charity>
    {
        public void Configure(EntityTypeBuilder<Charity> builder)
        {
            builder.Property(c => c.Name)
             .IsRequired()
             .HasMaxLength(100);

            
            builder.Property(c => c.Description)
                .IsRequired()
                .HasMaxLength(500); 

            
            builder.Property(c => c.ImageURL)
                .IsRequired(false); 

            
            builder.Property(c => c.Phone)
                .IsRequired()
                .HasMaxLength(15);


            builder.Property(c => c.Is_Active)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(c => c.CreationDate).HasDefaultValueSql("GETDATE()");
        }
    }
}
