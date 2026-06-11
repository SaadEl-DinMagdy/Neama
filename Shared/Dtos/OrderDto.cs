
using Shared.shareEnumsAndEntitys;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos
{
    public class OrderDto
    {
        [Required]
        public string BasketId { get; set; }

        [Required]
        public int DeliveryMethodId { get; set; }

        public AddressDto ShippingAddress { get; set; }


        [Required]
        public PaymentMethodType PaymentMethod { get; set; } 
    }
}
