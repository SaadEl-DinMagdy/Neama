
using Microsoft.Extensions.Configuration;
using Neama.Core;
using Neama.Core.Entities;
using Neama.Core.Entities.Order_Aggregate;
using Neama.Core.Repositories.Contract;
using Neama.Core.Services.Contract;
using Neama.Core.Specifications;
using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Service.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IBasketRepository _basketRepo;

        public PaymentService(IUnitOfWork unitOfWork, IConfiguration configuration, IBasketRepository basketRepo)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _basketRepo = basketRepo;
        }

        public async Task<CustomerBasket?> CreateOrUpdatePaymentIntent(string basketId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

            var basket = await _basketRepo.GetAsync(basketId);
            if (basket is null) return null;

            var shippingPrice = 0m;

            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(basket.DeliveryMethodId.Value);
                basket.ShippingPrice = deliveryMethod.Cost;
                shippingPrice = deliveryMethod.Cost;
            }

            if (basket?.Items.Count > 0)
            {
                foreach (var item in basket.Items)
                {
                    var product = await _unitOfWork.Repository<Item>().GetAsync(item.Id);
                    if (product is null) return null;
                    if (item.Price != product.OriginalPrice)
                        item.Price = product.OriginalPrice;
                }
            }

            PaymentIntentService paymentIntentService = new PaymentIntentService();
            PaymentIntent paymentIntent;

            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                var createOptions = new PaymentIntentCreateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Price * 100 * item.Quantity) + (long)shippingPrice * 100,
                    Currency = "egp",
                    PaymentMethodTypes = new List<string>() { "card" }
                };

                paymentIntent = await paymentIntentService.CreateAsync(createOptions);
                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                var updateOptions = new PaymentIntentUpdateOptions()
                {
                    Amount = (long)basket.Items.Sum(item => item.Price * 100 * item.Quantity) + (long)shippingPrice * 100
                };

                await paymentIntentService.UpdateAsync(basket.PaymentIntentId, updateOptions);
            }

            await _basketRepo.SetAsync(basketId,basket,TimeSpan.FromDays(1));
            return basket;
        }

        public async Task<Order> UpdatePaymentIntentToSucceededOrFailed(string paymentIntentId, bool isSucceeded)
        {
            var spec = new BaseSpecifications<Order>(O => O.PaymentIntenId == paymentIntentId);
            var order = await _unitOfWork.Repository<Order>().GetWithSpecAsync(spec);

            if (isSucceeded)
                order.Status = OrderStatus.PaymentReceived;
            else
                order.Status = OrderStatus.PaymentFailed;

            _unitOfWork.Repository<Order>().Update(order);
            await _unitOfWork.CompleteAsync();

            return order;
        }
    }
}
