using Neama.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Specifications.ReviewsByBranchSpecification
{
    public class ReviewsByBranchSpecification : BaseSpecifications<Review>
    {
        public ReviewsByBranchSpecification(int branchId)
            : base(r => r.BranchId == branchId)
        {
            Includes.Add(r => r.User);
            AddOrderByDesc(r => r.CreationDate);
        }
        
    }
}
