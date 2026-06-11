using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neama.Api.Errors;
using Neama.Core.Entities;
using Neama.Core.Services.Contract;
using Shared.Dtos;

namespace Neama.Api.Controllers
{
    [Authorize]
    public class BasketController : BaseApiController
    {
        private readonly IBasketService _basketService;

        public BasketController(IBasketService basketService)
        {
            _basketService = basketService;
        }

        [HttpGet]
        public async Task<ActionResult<CustomerBasket>> GetBasket(string id)
        {
            var basket = await _basketService.GetBasketAsync(id);
            return Ok(basket);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(string id, CustomerBasketDto basketDto)
        {
            if (basketDto.Items.Any())
            {
                var BranchId = basketDto.Items.First().BranchId;
                var cheak = basketDto.Items.Any(i => i.BranchId != BranchId);

                if (cheak)
                {
                    return BadRequest(new ApiResponse(400, "لا يمكن ان تحتوى السله على منتجات من اماكن مختلفه يجب بدء سله جديده"));
                }

            }

            var result = await _basketService.UpdateBasketAsync(id, basketDto);

            if (result == null) return BadRequest(new ApiResponse(400,"حدث خطاء اثناء تعديل السله"));

            return Ok(result);
        }

        [HttpDelete] 
        public async Task DeleteBasket(string id)
        {
            await _basketService.DeleteBasketAsync(id);
        }
    }
}
