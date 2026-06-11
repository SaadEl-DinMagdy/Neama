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
    internal class AddressConfigration : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {



            builder.Property(a => a.Latitude).IsRequired();
            builder.Property(a => a.Longitude).IsRequired();

            
            builder.Property(a => a.Street).IsRequired().HasMaxLength(100);
            builder.Property(a => a.City).IsRequired().HasMaxLength(50);

            
            builder.Property(a => a.Details)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
