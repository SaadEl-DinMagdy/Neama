using Neama.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Repositories.Contract
{
    public interface IBasketRepository : IRedisRepository<CustomerBasket> 
    {
    }
}
