using Neama.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Specifications.AddressSpecifications
{
    public class AddresswhereandsortSpecification : BaseSpecifications<Address>
    {
        public AddresswhereandsortSpecification(string userId, int id)
            : base(a =>
                  (a.AppUserId == userId && a.Id == id)
            )
        {
            
        }

        public AddresswhereandsortSpecification(string userId)
            : base(a =>
                  (a.AppUserId == userId )
            )
        {
            AddOrderByDesc(a => a.CreatedDate);
        }
    }
}
