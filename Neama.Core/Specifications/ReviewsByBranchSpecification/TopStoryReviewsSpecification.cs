using Neama.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Specifications.ReviewsByBranchSpecification
{
    public class TopStoryReviewsSpecification : BaseSpecifications<Review>
    {
        public TopStoryReviewsSpecification(int take)
            : base(r => r.ImageUrl != null) 
        {
            Includes.Add(r => r.User);
            AddOrderByDesc(r => r.CreationDate); 
            ApplayPagination(0, take); 
        }
    }
}
