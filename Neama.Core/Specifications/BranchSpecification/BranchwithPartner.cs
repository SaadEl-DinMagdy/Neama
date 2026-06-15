using Microsoft.EntityFrameworkCore.Query;
using Neama.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Specifications.BranchSpecification
{
    public class BranchwithPartner : BaseSpecifications<Branch>
    {
        public BranchwithPartner(int Id): base(B=> B.Id==Id&& B.Is_Active)
        {
            Includes.Add(B => B.Partner);
        }
    }
}
