using Neama.Core;
using Neama.Core.Entities;
using Neama.Core.Entities.Order_Aggregate;
using Neama.Core.Repositories.Contract;
using Neama.Core.Services.Contract;
using Neama.Core.Specifications;
using Neama.Core.Specifications.BranchSpecification;
using Neama.Core.Specifications.OrderSpecifications;
using Shared.Dtos;
using Shared.shareEnumsAndEntitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Service.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBasketRepository _basketRepository;
        private readonly IPaymentService _paymentService;
        private readonly ILocationService _locationService;

        public OrderService(IUnitOfWork unitOfWork, IBasketRepository basketRepository, IPaymentService paymentService , ILocationService locationService)
        {
            _unitOfWork = unitOfWork;
            _basketRepository = basketRepository;
            _paymentService = paymentService;
            _locationService = locationService;
        }

        public async Task<object> CreateOrderAsync(string buyerEmail, string basketId, int deliveryMethodId, AddressDto? address, PaymentMethodType paymentMethod)
        {

            var Basket = await _basketRepository.GetAsync(basketId);
            if (Basket == null) return OrderResult.Failure("السله عير موجوده");
            if (Basket.Items.Count == 0) return OrderResult.Failure("السله فارغه");

            var orderItems = new List<OrderItem>();
            int totalSavedMeals = 0;
            decimal totalSavedAmount = 0;


            var branchId = Basket.Items[0].BranchId;
            var branch = await _unitOfWork.Repository<Branch>().GetAsync(branchId);
            if (branch == null || branch.Is_Active==false) return OrderResult.Failure("الفرع غير موجود");

            var partner = await _unitOfWork.Repository<Partner>().GetAsync(branch.PartnerId ?? 0);

            if (deliveryMethodId == 1)
            {
                if(address == null) return OrderResult.Failure("يجب ادخال عنوان التوصيل");
                var userLocation = LocationHelper.GetUserPoint(address.Longitude, address.Latitude);
                if (branch.Location.Distance(userLocation) > 5000)
                    return OrderResult.Failure("عنوان التوصيل لا يقع فى حدود التوصيل اقصى مسافه 5 كيلو متر من الفرع");
            }


            foreach (var item in Basket.Items)
            {
                var product = await _unitOfWork.Repository<Item>().GetAsync(item.Id);
                if (product == null) return OrderResult.Failure($"المنتج  {item.ProductName} غير متوافر");

                if (product.StockQuantity < item.Quantity)
                    return OrderResult.Failure($"عفوا,{product.StockQuantity} وحده متبقيه من  {product.Name}");

                totalSavedAmount += (product.OriginalPrice - product.DiscountPrice) * item.Quantity;
                totalSavedMeals += item.Quantity;

                var productItemOrdered = new ProductItemOrdered(item.Id, product.Name, product.ImageURL);
                orderItems.Add(new OrderItem(productItemOrdered, product.DiscountPrice, item.Quantity));
            }


            var SubTotal = orderItems.Sum(O => O.Quantity * O.Price);
            var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(deliveryMethodId);
            var pickupCode = new Random().Next(100000, 999999).ToString();
            ShippingAddress? shippingAddress = null;
            if (deliveryMethodId == 1)
            {
                var location = await _locationService.GetAddressAsync(address.Latitude, address.Longitude);
                shippingAddress = new ShippingAddress(address.Name, address.PhoneNumber, address.Longitude, address.Latitude, address.DistinctiveMark, location.Street, location.City, location.Governorate, location.DisplayName);
            }
            var intentId = paymentMethod == PaymentMethodType.Card ? Basket?.PaymentIntentId : null;

            var order = new Order(branchId, partner.Id,buyerEmail, shippingAddress, DeliveryMethod, orderItems, SubTotal, paymentMethod, intentId)
            {
                VerificationCode = pickupCode,
                SavedMealsCount = totalSavedMeals,
                TotalSavedAmount = totalSavedAmount,
                PartnerLogoUrl = partner?.Logo_URL,
                PartnerCoverUrl = partner?.Cover_URL
            };

            await _unitOfWork.Repository<Order>().AddAsync(order);

            foreach (var item in Basket.Items)
            {
                var product = await _unitOfWork.Repository<Item>().GetAsync(item.Id);
                if (product != null)
                {
                    product.StockQuantity -= item.Quantity;
                    _unitOfWork.Repository<Item>().Update(product);
                }
            }


            

            if(paymentMethod == PaymentMethodType.Card)
            {
                var total = SubTotal + DeliveryMethod.Cost;
                partner.WalledBalance += total-(SubTotal * 0.10m);
                 _unitOfWork.Repository<Partner>().Update(partner);
            }
            else
            {
                partner.WalledBalance -= SubTotal * 0.10m;
                _unitOfWork.Repository<Partner>().Update(partner);
            }
            var result = await _unitOfWork.CompleteAsync();
            if (result <= 0) return OrderResult.Failure("حدث خطا اثناء حفظ الطلب");
            await _basketRepository.DeleteAsync(basketId);

            return OrderResult.Success(order);
        }

        public async Task<Order?> GetOrderByIdForUserAsync(int orderId, string buyerEmail)
        {
            var spec = new OrderSpec(buyerEmail, orderId);
            var orders = await _unitOfWork.Repository<Order>().GetWithSpecAsync(spec);
            return orders;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersForUserAsync(string buyerEmail)
        {
            var spec = new OrderSpec(buyerEmail);
            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
            return orders;
        }

        public async Task<IReadOnlyList<DeliveryMethod>> GetDeliveryMethodsAsync()
        {
            var data = await _unitOfWork.Repository<DeliveryMethod>().GetAllAsync();
            return data;
        }
    }
}

