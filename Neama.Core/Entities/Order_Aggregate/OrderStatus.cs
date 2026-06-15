using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Entities.Order_Aggregate
{
    public enum OrderStatus
    {
        [EnumMember(Value = "Pending")]
        Pending,             

        [EnumMember(Value = "Payment Received")]
        PaymentReceived,      

        [EnumMember(Value = "Payment Failed")]
        PaymentFailed,        

        [EnumMember(Value = "Preparing")]
        Preparing,            

        [EnumMember(Value = "Out For Delivery")]
        OutForDelivery,       

        [EnumMember(Value = "Delivered")]
        Delivered,            

        [EnumMember(Value = "Cancelled")]
        Cancelled             
    }
}
