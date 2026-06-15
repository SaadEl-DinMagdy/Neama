using Neama.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Specifications.UserMarketSpecification
{
    public class UserMarketSearchandSortOnlySpec : BaseSpecifications<UserProduct>
    {
        public UserMarketSearchandSortOnlySpec(string userId , string? Search)
            : base(m =>
                  (string.IsNullOrEmpty(Search) || m.Name.ToLower().Contains(Search)) && m.AppUserId == userId
            )
        {
            AddOrderByDesc(m => m.CreationDate);
        }
    }
}
