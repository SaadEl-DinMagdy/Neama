using Neama.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Specifications.PartnerSpecifications
{
    public class PartnerWithBranchesSpecification : BaseSpecifications<Partner>
    {
        public PartnerWithBranchesSpecification(int id) 
            :base(P => P.Is_Active&& P.Id == id)
        {
            Includes.Add(P => P.MainSection);
            Includes.Add(P => P.Manager);
            Includes.Add(P => P.Branches);
        }

        public PartnerWithBranchesSpecification(string? search)
           : base(P => P.Is_Active && (string.IsNullOrEmpty(search) || P.Name.ToLower().Contains(search)))
        {
            Includes.Add(P => P.MainSection);
            Includes.Add(P => P.Manager);
        }
    }
}
