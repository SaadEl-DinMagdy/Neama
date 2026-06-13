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
    internal class PartnerConfigration : IEntityTypeConfiguration<Partner>
    {
        public void Configure(EntityTypeBuilder<Partner> builder)
        {
            builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

            builder.Property(p => p.WalledBalance)
                .HasColumnType("decimal(18,2)")
                .HasDefaultValue(0m);

            builder.Property(p => p.Is_Active)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(p => p.Logo_URL).IsRequired(false);
            builder.Property(p => p.Cover_URL).IsRequired(false);

            builder.Property(p => p.CreationDate).HasDefaultValueSql("GETDATE()");


            builder.HasMany(p => p.Branches)
                .WithOne(b => b.Partner)
                .HasForeignKey(b => b.PartnerId)
                .OnDelete(DeleteBehavior.SetNull);



            builder.HasOne(p => p.MainSection)
                .WithMany(s => s.Partners)
                .HasForeignKey(p => p.MainSectionId)
                .OnDelete(DeleteBehavior.SetNull);


        }
    }
}
