using Neama.Core.Entities;
using NetTopologySuite;
using NetTopologySuite.Geometries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Specifications.BranchSpecification
{
    public class BranchWithSearchAndSortSpec : BaseSpecifications<Branch>
    {
        
        public BranchWithSearchAndSortSpec(BranchParams SpecParams) :
            base
            (
                B =>
                ((string.IsNullOrEmpty(SpecParams.Search) || B.BranchName.ToLower().Contains(SpecParams.Search)) &&
                 B.Is_Active && B.Partner.MainSectionId == SpecParams.MainSectionId
                 &&B.Location != null &&
                B.Location.Distance(LocationHelper.GetUserPoint(SpecParams.Longitude,SpecParams.Latitude))<=SpecParams.DistanceKiloMeter*1000
                 ) 
            )
        {
            Includes.Add(B => B.Partner);
            Includes.Add(B => B.Items);

            if (!string.IsNullOrEmpty(SpecParams.Sort))
            {
                switch (SpecParams.Sort.ToLower())
                {
                    case "location":
                        AddOrderByAsc(B => B.Location.Distance(
                        LocationHelper.GetUserPoint(SpecParams.Longitude, SpecParams.Latitude)));
                        break;
                    case "rate":
                        AddOrderByDesc(B => B.AverageRating);
                        break;
                    default:
                        AddOrderByAsc(B => B.BranchName);
                        break;
                }
            }
            else
            {
                AddOrderByAsc(B => B.BranchName);
            }

            ApplayPagination((SpecParams.PageIndex - 1) * SpecParams.PageSize, SpecParams.PageSize);


        }

        
    }
}
