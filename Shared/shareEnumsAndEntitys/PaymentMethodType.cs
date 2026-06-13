using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Shared.shareEnumsAndEntitys
{
    public enum PaymentMethodType
    {
        [EnumMember(Value = "Cash")]
        Cash,

        [EnumMember(Value = "Card")]
        Card
    }
}
