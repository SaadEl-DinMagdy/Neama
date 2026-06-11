using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Neama.Core.Entities.Order_Aggregate;
using Shared.shareEnumsAndEntitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Repository.Data.Configrations
{
    internal class OrderConfigration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {

            builder.OwnsOne(O => O.ShippingAddress, A => A.WithOwner());


            builder.Property(O => O.Status)
                .HasConversion
                (
                 SetStatus => SetStatus.ToString(),
                 GetStatus => (OrderStatus)Enum.Parse(typeof(OrderStatus), GetStatus)
                );


            builder.Property(O => O.PaymentMethod)
                .HasConversion
                (
                 SetMethod => SetMethod.ToString(),
                 GetMethod => (PaymentMethodType)Enum.Parse(typeof(PaymentMethodType), GetMethod)
                );


            builder.Property(O => O.SubTotal)
                .HasColumnType("decimal(18,2)");


            builder.HasOne(O => O.DeliveryMethod)
                .WithMany()
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
