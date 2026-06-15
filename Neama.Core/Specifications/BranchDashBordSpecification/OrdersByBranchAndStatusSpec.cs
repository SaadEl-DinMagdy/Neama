using Neama.Core.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Specifications.BranchDashBordSpecification
{
    public class OrdersByBranchAndStatusSpec : BaseSpecifications<Order>
    {
        public OrdersByBranchAndStatusSpec(int branchId, OrderStatus? status)
            : base(o => o.BranchId == branchId && (!status.HasValue || o.Status == status))
        {
            Includes.Add(o => o.Items);
            AddOrderByDesc(o => o.OrderDate);
        }
    }
}
