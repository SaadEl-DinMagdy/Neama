using Neama.Core.Entities;
using Neama.Core.Repositories.Contract;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Repository
{
    public class BasketRepository : RedisRepository<CustomerBasket>, IBasketRepository
    {
        public BasketRepository(IConnectionMultiplexer redis) : base(redis)
        {
            
        }
    }
}
