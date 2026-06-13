using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neama.Api.Errors;
using Neama.Core.Services.Contract;
using Neama.Service.BranchService;
using Neama.Service.CategoryService;
using Shared.Dtos;

namespace Neama.Api.Controllers
{
    [Authorize]
    public class ItemController : BaseApiController
    {
        private readonly IItemService _itemService;
        private readonly IBranchService _branchService;

        public ItemController(IItemService itemService , IBranchService branchService)
        {
            _itemService = itemService;
            _branchService = branchService;
        }

        [HttpGet("BranchItems/{id}")]
        public async Task<ActionResult<IReadOnlyList<AllItemDto>>> GetItems(int id ,int? CategoryId)
        {

            var branch = await _branchService.GetAsync(id);
            if (branch != null)
            {
                var item = await _itemService.GetAllSpecAsync(id, CategoryId);
                return Ok(item);
            }

            return NotFound(new ApiResponse(404, "الفرع غير موجود"));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ItemDto>> GetItem(int id)
        {
            var item = await _itemService.GetItemAsync(id);

            if(item == null)
            {
                return NotFound(new ApiResponse(404));
            }

            return Ok(item);
        }
    }
}
