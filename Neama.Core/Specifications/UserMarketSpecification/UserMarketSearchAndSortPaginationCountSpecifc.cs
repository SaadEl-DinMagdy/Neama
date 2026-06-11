using Neama.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Specifications.UserMarketSpecification
{
    public class UserMarketSearchAndSortPaginationCountSpecifc : BaseSpecifications<UserProduct>
    {
        public UserMarketSearchAndSortPaginationCountSpecifc(MarketParam param)
            : base(m =>
                  (string.IsNullOrEmpty(param.Search) || m.Name.ToLower().Contains(param.Search))
            )
        {
           
        }
    }
}
