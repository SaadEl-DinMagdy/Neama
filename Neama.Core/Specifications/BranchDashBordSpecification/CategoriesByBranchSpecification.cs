using Neama.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Specifications.BranchDashBordSpecification
{
    public class CategoriesByBranchSpecification : BaseSpecifications<Category>
    {
        public CategoriesByBranchSpecification(int branchId)
            : base(c => c.BranchId == branchId)
        {
            AddOrderByAsc(c => c.Name);
        }
    }
}
