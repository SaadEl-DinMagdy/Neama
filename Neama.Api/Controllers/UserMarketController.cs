using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neama.Api.Errors;
using Neama.Api.Helper;
using Neama.Core;
using Neama.Core.Entities;
using Neama.Core.Services.Contract;
using Neama.Core.Specifications.BranchSpecification;
using Neama.Core.Specifications.UserMarketSpecification;
using Shared.Dtos;
using System.Security.Claims;

namespace Neama.Api.Controllers
{
    [Authorize]
    public class UserMarketController : BaseApiController
    {
        private readonly IUserMarketService _userMarketService;
        private readonly IUnitOfWork _unitOfWork;

        public UserMarketController(IUserMarketService userMarketService , IUnitOfWork unitOfWork)
        {
            _userMarketService = userMarketService;
            _unitOfWork = unitOfWork;
        }
        private string GetCurrentUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        


        [HttpGet("MyProducts")]
        public async Task<ActionResult<IReadOnlyList<UserProductDto>>> GetMyProducts(string? search)
        {
            var userId = GetCurrentUserId();
            var products = await _userMarketService.GetAllMyProductSpecAsync(userId, search);

            return Ok(products);
        }

        [HttpGet("MyProducts/{id}")]
        public async Task<ActionResult<UserProductDto>> GetMyProductById(int id)
        {
            var userId = GetCurrentUserId();
            var product = await _userMarketService.GetMyProductSpecAsync(userId, id);

            if (product == null )
            {
                return NotFound(new ApiResponse(404));
            }


            return Ok(product);
        }

        [HttpGet]
        public async Task<ActionResult<Pagination<ReturnUserProductDto>>> GetMarketProducts([FromQuery] MarketParam param)
        {
            var products = await _userMarketService.GetUserMarketSpecAsync(param);
            var spec = new UserMarketSearchAndSortPaginationCountSpecifc(param);
            var count = await _unitOfWork.Repository<UserProduct>().GetCountAsync(spec);
            return Ok(new Pagination<ReturnUserProductDto>(param.PageIndex,param.PageSize,count,products));
        }

        [HttpPost]
        public async Task<ActionResult> AddProduct([FromForm] AddUserProduct productDto)
        {

            var userId = GetCurrentUserId();
            var success = await _userMarketService.AddAsync(productDto,userId);

            return Ok();
        }


        [HttpPut]
        public async Task<ActionResult<ReturnUserProductDto>> UpdateProduct([FromForm] UserProductDto productDto)
        {

            var updatedProduct = await _userMarketService.Update(productDto);

            if (updatedProduct == null)
            {
                return NotFound(new ApiResponse (404, "Product not found or could not be updated." ));
            }

            return Ok(updatedProduct);
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {

            var success = await _userMarketService.Remove(id);

            return Ok();
        }
    }
}
