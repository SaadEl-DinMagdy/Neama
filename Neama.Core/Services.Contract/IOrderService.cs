using Neama.Core.Entities.Order_Aggregate;
using Shared.Dtos;
using Shared.shareEnumsAndEntitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Services.Contract
{
    public interface IOrderService
    {
        Task<object?> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethod, AddressDto address, PaymentMethodType paymentMethod);

        Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail);

        Task<Order?> GetOrderByIdForUserAsync(int orderId, string buyerEmail);

        Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync();
    }
}
