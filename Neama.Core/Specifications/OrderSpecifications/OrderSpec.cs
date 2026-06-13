using Neama.Core.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Specifications.OrderSpecifications
{
    public class OrderSpec : BaseSpecifications<Order>
    {
        public OrderSpec(string email)
            : base(O =>
            O.BuyerEmail == email)
        {
            Includes.Add(o => o.Items);
            Includes.Add(o => o.DeliveryMethod);

            AddOrderByDesc(o => o.OrderDate);
        }

        public OrderSpec(string email, int orderId)
            : base(O =>
            O.BuyerEmail == email && O.Id == orderId)
        {
            Includes.Add(o => o.Items);
            Includes.Add(o => o.DeliveryMethod);
        }

        public OrderSpec(string email,bool profile)
            : base(O =>
            O.BuyerEmail == email)
        {
          
        }


    }
}
