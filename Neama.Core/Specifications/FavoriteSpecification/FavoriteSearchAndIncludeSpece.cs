using Neama.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Specifications.FavoriteSpecification
{
    public class FavoriteSearchAndIncludeSpece : BaseSpecifications<Favorite>
    {
        public FavoriteSearchAndIncludeSpece(string userId ,string? Search)
            : base(f =>
                  (string.IsNullOrEmpty(Search) || f.Branch.BranchName.ToLower().Contains(Search))&& f.Branch.Is_Active&&userId == f.UserId
            )
        {
            Includes.Add(f=>f.Branch);
            Includes.Add(f => f.Branch.Partner);
        }
    }
}
