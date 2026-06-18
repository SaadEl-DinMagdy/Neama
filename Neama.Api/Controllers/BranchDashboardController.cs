using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neama.Api.Errors;
using Neama.Core.Entities.Order_Aggregate;
using Neama.Core.Services.Contract;
using Neama.Service.PartnerDashboardService;
using Shared;
using Shared.Dtos;
using Shared.shareEnumsAndEntitys;
using System.Security.Claims;

namespace Neama.Api.Controllers
{
    [Authorize(Roles = AppRoles.Branch)]
    public class BranchDashboardController : BaseApiController
    {
        private readonly IBranchDashboardService _dashboardService;
        private readonly IBranchService _branchService;
        private readonly IOrderService _orderService;

        public BranchDashboardController(IBranchDashboardService dashboardService , IBranchService branchService )
        {
            _dashboardService = dashboardService;
            _branchService = branchService;
        }

        private async Task<int?> GetCurrentBranchIdAsync()
        {
            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(managerId)) return null;

            return await _dashboardService.GetBranchIdByManagerAsync(managerId);
        }

        [HttpGet("info")]
        public async Task<ActionResult<BranchDto>> GetInfo()
        {
            var branchId = await GetCurrentBranchIdAsync();
            if (branchId == null)
                return Unauthorized(new ApiResponse(401, "عفواً، حسابك غير مسجل كمدير لأي فرع."));

           var result = await _branchService.GetAsync(branchId.Value);
            return Ok(result);
        }
        [HttpGet("items")]
        public async Task<ActionResult<IReadOnlyList<ItemDashbordDto>>> GetItems(string? searchName)
        {
            var branchId = await GetCurrentBranchIdAsync();
            if (branchId == null)
                return Unauthorized(new ApiResponse(401, "عفواً، حسابك غير مسجل كمدير لأي فرع."));

            var items = await _dashboardService.GetAllItemsAsync(branchId.Value, searchName);
            return Ok(items);
        }

        [HttpGet("items/{id}")]
        public async Task<ActionResult<ItemDashbordDto>> GetItemById(int id)
        {
            var item = await _dashboardService.GetItemByIdAsync(id);
            if (item == null)
                return NotFound(new ApiResponse(404, "عفواً، هذا المنتج غير موجود."));

            return Ok(item);
        }

        [HttpPost("items")]
        public async Task<ActionResult> AddItem([FromForm] ItemCreateDto model)
        {
            var branchId = await GetCurrentBranchIdAsync();
            if (branchId == null)
                return Unauthorized(new ApiResponse(401, "عفواً، حسابك غير مسجل كمدير لأي فرع."));

            var result = await _dashboardService.AddItemAsync(branchId.Value, model);
            if (!result)
                return BadRequest(new ApiResponse(400, "حدث خطأ أثناء إضافة المنتج، يرجى المحاولة مرة أخرى."));

            return Ok(new ApiResponse(200, "تمت إضافة المنتج بنجاح."));
        }

        [HttpPut("items/{id}")]
        public async Task<ActionResult> UpdateItem(int id, [FromForm] ItemUpdateDto model)
        {
            var result = await _dashboardService.UpdateItemAsync(id, model);
            if (!result)
                return BadRequest(new ApiResponse(400, "حدث خطأ أثناء تعديل بيانات المنتج."));

            return Ok(new ApiResponse(200, "تم تعديل المنتج بنجاح."));
        }

        [HttpDelete("items/{id}")]
        public async Task<ActionResult> DeleteItem(int id)
        {
            var result = await _dashboardService.DeleteItemAsync(id);
            if (!result)
                return BadRequest(new ApiResponse(400, "حدث خطأ أثناء حذف المنتج."));

            return Ok(new ApiResponse(200, "تم حذف المنتج بنجاح."));
        }


        [HttpGet("categories")]
        public async Task<ActionResult<IReadOnlyList<CategoryDto>>> GetCategories()
        {
            var branchId = await GetCurrentBranchIdAsync();
            if (branchId == null)
                return Unauthorized(new ApiResponse(401, "عفواً، حسابك غير مسجل كمدير لأي فرع."));

            var categories = await _dashboardService.GetCategoriesByBranchAsync(branchId.Value);
            return Ok(categories);
        }

        [HttpPost("categories")]
        public async Task<ActionResult> AddCategory(string Name)
        {
            var branchId = await GetCurrentBranchIdAsync();
            if (branchId == null)
                return Unauthorized(new ApiResponse(401, "عفواً، حسابك غير مسجل كمدير لأي فرع."));

            var result = await _dashboardService.AddCategoryAsync(branchId.Value, Name);
            return result
                ? Ok(new ApiResponse(200, "تمت إضافة القسم بنجاح."))
                : BadRequest(new ApiResponse(400, "حدث خطأ أثناء إضافة القسم."));
        }

        [HttpPut("categories")]
        public async Task<ActionResult> UpdateCategory( CategoryDto model)
        {
            var result = await _dashboardService.UpdateCategoryAsync( model);
            return result
                ? Ok(new ApiResponse(200, "تم تعديل القسم بنجاح."))
                : BadRequest(new ApiResponse(400, "حدث خطأ أثناء تعديل القسم."));
        }

        [HttpDelete("categories/{id}")]
        public async Task<ActionResult> DeleteCategory(int id)
        {
            var result = await _dashboardService.DeleteCategoryAsync(id);
            return result
                ? Ok(new ApiResponse(200, "تم حذف القسم بنجاح."))
                : NotFound(new ApiResponse(404, "القسم غير موجود"));
        }

        
        [HttpGet("orders")]
        public async Task<ActionResult> GetOrders([FromQuery] OrderStatus? status)
        {
            var branchId = await GetCurrentBranchIdAsync();
            if (branchId == null)
                return Unauthorized(new ApiResponse(401, "عفواً، حسابك غير مسجل كمدير لأي فرع."));

            var orders = await _dashboardService.GetOrdersFilteredAsync(branchId.Value, status);
            return Ok(orders);
        }

        [HttpPut("orders/{orderId}/status")]
        public async Task<ActionResult> ChangeOrderStatus(int orderId, [FromQuery] OrderStatus status)
        {
            var result = await _dashboardService.ChangeOrderStatusAsync(orderId, status);
            if (!result)
                return BadRequest(new ApiResponse(400, "حدث خطأ أثناء تغيير حالة الطلب."));

            return Ok(new ApiResponse(200, "تم تحديث حالة الطلب بنجاح."));
        }

        [HttpGet("report")]
        public async Task<ActionResult<BranchReportDto>> GetReport([FromQuery] ReportTimeFilter filter = ReportTimeFilter.ThisMonth)
        {
            var branchId = await GetCurrentBranchIdAsync();
            if (branchId == null)
                return Unauthorized(new ApiResponse(401, "عفواً، حسابك غير مسجل كمدير لأي فرع."));

            var report = await _dashboardService.GetBranchReportAsync(branchId.Value, filter);
            return Ok(report);
        }
        [HttpGet("profitgrowth")]
        public async Task<ActionResult<YearlyGrowthReportDto>> GetProfitGrowth([FromQuery] int year = 2026)
        {
            var branchid = await GetCurrentBranchIdAsync();
            var result = await _dashboardService.GetProfitGrowthAsync(branchid.Value, year);
            return Ok(result);
        }
        [HttpGet("itemsgrowth")]
        public async Task<ActionResult<YearlyGrowthReportDto>> GetItemsGrowth([FromQuery] int year = 2026)
        {
            var branchid = await GetCurrentBranchIdAsync();
            var result = await _dashboardService.GetItemsbuyGrowthAsync(branchid.Value, year);
            return Ok(result);
        }
    }
}
