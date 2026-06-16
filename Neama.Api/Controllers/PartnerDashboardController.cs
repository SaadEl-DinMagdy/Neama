using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neama.Api.Errors;
using Neama.Core.Services.Contract;
using Shared;
using Shared.Dtos;
using Shared.shareEnumsAndEntitys;
using System.Security.Claims;

namespace Neama.Api.Controllers
{
    [Authorize(Roles = AppRoles.Partner)] 
    public class PartnerDashboardController : BaseApiController
    {
        private readonly IPartnerDashboardService _partnerDashboardService;

        public PartnerDashboardController(IPartnerDashboardService partnerDashboardService)
        {
            _partnerDashboardService = partnerDashboardService;
        }

        private async Task<int?> GetCurrentPartnerIdAsync()
        {
            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            var partner = await _partnerDashboardService.GetPartnerByManagerIdAsync(managerId);
            return partner?.Id;
        }

        [HttpPost("branch")]
        public async Task<ActionResult> AddBranch(AddBranchDto model)
        {
            var partnerId = await GetCurrentPartnerIdAsync();

            var result = await _partnerDashboardService.AddBranchAsync(partnerId.Value, model);
            if (!result) return BadRequest(new ApiResponse(400, "حدث خطأ، قد يكون الإيميل مستخدماً."));

            return Ok("تم إضافة الفرع .");
        }

        [HttpDelete("branch/{id}")]
        public async Task<ActionResult> RemoveBranch(int id)
        {
            var partnerId = await GetCurrentPartnerIdAsync();

            var result = await _partnerDashboardService.RemoveBranchAsync(id, partnerId.Value);
            if (!result) return NotFound(new ApiResponse(404, "الفرع غير موجود."));

            return Ok("تم إيقاف الفرع بنجاح.");
        }

        [HttpPut("partner")]
        public async Task<ActionResult> UpdatePartner(UpdatePartnerDto model)
        {
            var partnerId = await GetCurrentPartnerIdAsync();


            var result = await _partnerDashboardService.UpdatePartnerAsync(partnerId.Value, model);
            if (!result) return BadRequest(new ApiResponse(400));

            return Ok("تم تحديث البيانات بنجاح.");
        }

        [HttpGet("branches")]
        public async Task<ActionResult<IReadOnlyList<AllBranchResponseDto>>> GetAllBranches()
        {
            var partnerId = await GetCurrentPartnerIdAsync();


            var branches = await _partnerDashboardService.GetAllBranchesAsync(partnerId.Value);

            return Ok(branches);
        }

        [HttpGet("branch/{id}")]
        public async Task<ActionResult<BranchDetailsDto>> GetBranchDetails(int id, [FromQuery] ReportTimeFilter filter = ReportTimeFilter.AllTime, [FromQuery] int? year = null)
        {
            var partnerId = await GetCurrentPartnerIdAsync();

            var details = await _partnerDashboardService.GetBranchDetailsAsync(id, partnerId.Value, filter, year);
            if (details == null) return NotFound(new ApiResponse(404));

            return Ok(details);
        }

        [HttpGet("reports")]
        public async Task<ActionResult<PartnerReportDto>> GetReports([FromQuery] ReportTimeFilter filter = ReportTimeFilter.ThisMonth, [FromQuery] int? year = null)
        {
            var partnerId = await GetCurrentPartnerIdAsync();


            var reports = await _partnerDashboardService.GetPartnerReportsAsync(partnerId.Value, filter, year);
            return Ok(reports);
        }
        [HttpGet("profitgrowth")]
        public async Task<ActionResult<YearlyGrowthReportDto>> GetProfitGrowth([FromQuery] int year = 2026)
        {
            var partnerId = await GetCurrentPartnerIdAsync();
            var result = await _partnerDashboardService.GetProfitGrowthAsync(partnerId.Value, year);
            return Ok(result);
        }
    }
}
