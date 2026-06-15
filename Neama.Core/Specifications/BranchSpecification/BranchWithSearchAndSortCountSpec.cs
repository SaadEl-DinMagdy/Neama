using Neama.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Specifications.BranchSpecification
{
    public class BranchWithSearchAndSortCountSpec : BaseSpecifications<Branch>
    {
        public BranchWithSearchAndSortCountSpec(BranchParams SpecParams) :
            base
            (
                B =>
                ((string.IsNullOrEmpty(SpecParams.Search) || B.BranchName.ToLower().Contains(SpecParams.Search)) &&
                 B.Is_Active && B.Partner.MainSectionId == SpecParams.MainSectionId
                 && B.Location != null && B.Location.Distance(LocationHelper.GetUserPoint(SpecParams.Longitude, SpecParams.Latitude)) <= SpecParams.DistanceKiloMeter * 1000)
            )
        { }
    }
}
