using Shared.shareEnumsAndEntitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace Shared.Dtos
{
    public class OrderToReturnDto
    {
        public int BranchId { get; set; }
        public int Id { get; set; }
        public string BuyerEmail { get; set; }
        public DateTimeOffset OrderDate { get; set; }

        public ShippingAddress ShippingAddress { get; set; }

        public string Status { get; set; } 

        public string DeliveryMethod { get; set; }
        public decimal DeliveryMethodCost { get; set; }

        public ICollection<OrderItemDto> Items { get; set; } = new HashSet<OrderItemDto>();

        public decimal SubTotal { get; set; }
        public decimal Total { get; set; }

        public string PaymentMethod { get; set; } 
        public string? PaymentIntenId { get; set; }
        public string VerificationCode { get; set; }
        public int SavedMealsCount { get; set; }
        public decimal TotalSavedAmount { get; set; }
        public string PartnerLogoUrl { get; set; }
        public string PartnerCoverUrl { get; set; }
    }
}
