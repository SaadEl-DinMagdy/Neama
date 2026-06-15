using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neama.Api.Errors;
using Neama.Core.Services.Contract;
using Shared.Dtos;

namespace Neama.Api.Controllers
{
    [Authorize]
    public class CategoryController : BaseApiController
    {
        private readonly ICategoryService _categoryService;
        private readonly IBranchService _branchService;

        public CategoryController(ICategoryService categoryService , IBranchService branchService)
        {
            _categoryService = categoryService;
            _branchService = branchService;
        }

        [HttpGet("{BranchId}")]
        public async Task<ActionResult<IReadOnlyList<CategoryDto>>> GetCategories(int BranchId)
        {
            var branch = await _branchService.GetAsync(BranchId);
            if (branch != null)
            {
                var Category = await _categoryService.GetBranchCategoriesAsync(BranchId);
                return Ok(Category);
            }

            return NotFound(new ApiResponse(404 , "الفرع غير موجود"));

        }
    }
}
