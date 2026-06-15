using Neama.Core.Entities;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Services.Contract
{
    public interface IBasketService
    {
        Task<CustomerBasket> GetBasketAsync(string id);
        Task<CustomerBasket?> UpdateBasketAsync(string id, CustomerBasketDto basket);
        Task DeleteBasketAsync(string Id);
    }
}
