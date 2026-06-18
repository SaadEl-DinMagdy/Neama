using Shared.shareEnumsAndEntitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Entities.Order_Aggregate
{
    public class Order : BaseEntity
    {
        public Order() { }

        public Order(int branchId, int partnerId,string buyerEmail, Shared.shareEnumsAndEntitys.ShippingAddress? shippingAddress, DeliveryMethod deliveryMethod, ICollection<OrderItem> items, decimal subTotal, PaymentMethodType paymentMethod, string? paymentIntenId = null)
        {
            BranchId = branchId;
            PartnerId = partnerId;
            BuyerEmail = buyerEmail;
            ShippingAddress = shippingAddress;
            DeliveryMethod = deliveryMethod;
            Items = items;
            SubTotal = subTotal;
            PaymentMethod = paymentMethod;
            PaymentIntenId = paymentIntenId;
        }
        public int BranchId { get; set; }
        public int PartnerId { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; } = DateTimeOffset.UtcNow;
        public Shared.shareEnumsAndEntitys.ShippingAddress? ShippingAddress { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Pending;
        public DeliveryMethod? DeliveryMethod { get; set; }
        public ICollection<OrderItem> Items { get; set; } = new HashSet<OrderItem>();
        public decimal SubTotal { get; set; }
        public decimal GetTotal() => SubTotal + DeliveryMethod.Cost;


        public PaymentMethodType PaymentMethod { get; set; }
        public string? PaymentIntenId { get; set; }

        public string VerificationCode { get; set; } 
        public int SavedMealsCount { get; set; }     
        public decimal TotalSavedAmount { get; set; } 
        public string PartnerLogoUrl { get; set; }
        public string PartnerCoverUrl { get; set; }
    }
}
