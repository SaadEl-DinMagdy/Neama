using Neama.Core.Entities;
using Neama.Core.Repositories.Contract;
using Neama.Core.Services.Contract;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Service.BasketService
{
    public class BasketService : IBasketService
    {
        private readonly IBasketRepository _basketRepo;

        public BasketService(IBasketRepository basketRepo)
        {
            _basketRepo = basketRepo;
        }
        public async Task DeleteBasketAsync(string Id)
        {
            await _basketRepo.DeleteAsync(Id);
        }

        

        public async Task<CustomerBasket> GetBasketAsync(string id)
        {
            var basket = await _basketRepo.GetAsync(id);
            return basket ?? new CustomerBasket(id);
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(string id, CustomerBasketDto basket)
        {
            var customerBasket = new CustomerBasket(id)
            {
                Items = basket.Items.Select(item => new BasketItem
                {
                    Id = item.Id,
                    ProductName = item.ProductName,
                    PictureUrl = item.PictureUrl,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    BranchId = item.BranchId, 
                }).ToList(),
                PaymentIntentId = basket.PaymentIntentId,
                DeliveryMethodId = basket.DeliveryMethodId,
                ShippingPrice = basket.ShippingPrice,
                ClientSecret = basket.ClientSecret

            };

            var result = await _basketRepo.SetAsync(id , customerBasket , TimeSpan.FromDays(1));

            return result;
        }
    }
}
