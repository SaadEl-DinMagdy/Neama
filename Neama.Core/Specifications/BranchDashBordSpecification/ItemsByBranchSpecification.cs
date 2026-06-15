using Neama.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Specifications.BranchDashBordSpecification
{
    public class ItemsByBranchSpecification : BaseSpecifications<Item>
    {
        public ItemsByBranchSpecification(int branchId, string? searchName)
            : base(i => i.BranchId == branchId &&
                       (string.IsNullOrEmpty(searchName) || i.Name.ToLower().Contains(searchName.ToLower()))&& i.IsActive==true)
        {
            Includes.Add(i => i.Category);
            AddOrderByDesc(i => i.StockQuantity);
        }

        public ItemsByBranchSpecification(int id) : base(i => i.Id == id && i.IsActive == true)
        {
            Includes.Add(i => i.Category);
        }
    }
}
