using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neama.Api.Errors;
using Neama.Core.Entities.Order_Aggregate;
using Neama.Core.Services.Contract;
using Neama.Service.OrderService;
using Shared.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Neama.Api.Controllers
{
    [Authorize] 
    public class OrdersController : BaseApiController
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder(OrderDto orderDto)
        {

            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(buyerEmail)) return Unauthorized(new ApiResponse(401));

     
            var result = await _orderService.CreateOrderAsync(buyerEmail, orderDto.BasketId, orderDto.DeliveryMethodId, orderDto.ShippingAddress, orderDto.PaymentMethod);

            if (result is OrderResult orderResult)
            {

                if (!orderResult.IsSuccess)
                    return BadRequest(new ApiResponse(400, orderResult.ErrorMessage));


                return Ok(MapOrderToReturnDto(orderResult.Order));
            }

            return BadRequest(new ApiResponse(400, "حدث خطاء اثناى اضافه الطلب"));
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetOrdersForUser()
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(buyerEmail)) return Unauthorized(new ApiResponse(401));

            var orders = await _orderService.GetOrdersForUserAsync(buyerEmail);

            var mappedOrders = orders.Select(order => MapOrderToReturnDto(order)).ToList();

            return Ok(mappedOrders);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderForUser(int id)
        {
            var buyerEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(buyerEmail)) return Unauthorized(new ApiResponse(401));

            var order = await _orderService.GetOrderByIdForUserAsync(id, buyerEmail);

            if (order == null) return NotFound(new ApiResponse(404, "الطلب غير موجود"));

            return Ok(MapOrderToReturnDto(order));
        }

        [HttpGet("deliveryMethod")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethod>>> GetDeliveryMethod()
        {
            var data = await _orderService.GetDeliveryMethodsAsync();
            return Ok(data);
        }


        private OrderToReturnDto MapOrderToReturnDto(Order order)
        {
            if (order == null) return null;

            return new OrderToReturnDto
            {
                BranchId = order.BranchId,
                Id = order.Id,
                BuyerEmail = order.BuyerEmail,
                OrderDate = order.OrderDate,
                ShippingAddress = order.ShippingAddress,

                Status = order.Status.ToString(),
                PaymentMethod = order.PaymentMethod.ToString(),

                DeliveryMethod = order.DeliveryMethod?.ShortName,
                DeliveryMethodCost = order.DeliveryMethod?.Cost ?? 0,

                SubTotal = order.SubTotal,
                Total = order.GetTotal(),
                PaymentIntenId = order.PaymentIntenId,


                VerificationCode = order.VerificationCode,
                SavedMealsCount = order.SavedMealsCount,
                TotalSavedAmount = order.TotalSavedAmount,
                PartnerLogoUrl = order.PartnerLogoUrl,
                PartnerCoverUrl = order.PartnerCoverUrl,


                Items = order.Items.Select(item => new OrderItemDto
                {
                    Id = item.Id,
                    ProductId = item.Product.ProductId,
                    ProductName = item.Product.ProductName,
                    PictureUrl = item.Product.PictureUrl,
                    Price = item.Price,
                    Quantity = item.Quantity
                }).ToList()
            };
        }
    }
}