using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Neama.Api.Errors;
using Neama.Core.Entities;
using Neama.Core.Services.Contract;
using Neama.Service.AdminDashboard;
using Neama.Service.MainSectionService;
using Neama.Service.PartnerService;
using Shared;
using Shared.Dtos;
using Shared.shareEnumsAndEntitys;

namespace Neama.Api.Controllers
{
    [Authorize(Roles = AppRoles.Admin)]
    public class AdminDashboardController : BaseApiController
    {

        private readonly IApplicationToJoinService _applicationService;
        private readonly IMainSectionService _mainSectionService;
        private readonly IAdminDashBoardService _admindashboard;
        private readonly IPartnerService _partnerService;

        public AdminDashboardController(IApplicationToJoinService applicationService , IMainSectionService mainSectionService , IAdminDashBoardService  admindashboard , IPartnerService partnerService)
        {
            _applicationService = applicationService;
            _mainSectionService = mainSectionService;
            _admindashboard = admindashboard;
            _partnerService = partnerService;
        }




        [HttpGet("reports")]
        public async Task<ActionResult<DashboardReportDto>> GetReports([FromQuery] ReportTimeFilter filter = ReportTimeFilter.ThisMonth,[FromQuery] int? year = null) 
        {
            var reports = await _admindashboard.GetDashboardReportsAsync(filter, year);

            return Ok(reports);
        }


        [HttpGet("ApplicationsToJoin")]
        public async Task<ActionResult<IReadOnlyList<ApplicationsToJoin>>> GetAllApplications(bool? ContactWasMade = false)
        {
            var applications = await _applicationService.GetAllApplicationsAsync(ContactWasMade);
            return Ok(applications);
        }


        [HttpPut("mark-contacted/{id}")]
        public async Task<ActionResult> MarkAsContacted(int id)
        {
            var result = await _applicationService.MarkAsContactedAsync(id);

            if (!result)
                return NotFound(new ApiResponse(404, "الطلب غير موجود أو حدث خطأ أثناء التحديث."));

            return Ok("تم تحديث حالة الطلب إلى 'تم التواصل' بنجاح." );
        }

        [HttpGet("AllUser")]
        public async Task<ActionResult<IReadOnlyList<UserInfoDto>>> GetAllUser()
        {
            var Data = await _admindashboard.GetAllUserAsync();
            return Ok(Data);
        }
        [HttpPost("mainsection")]
        public async Task<ActionResult> Add([FromForm] AddMainSectionDto model)
        {


            var result = await _mainSectionService.Add(model);

            if (!result)
                return BadRequest(new ApiResponse(400 , "حدث خطأ أثناء إضافة القسم."));

            return Ok("تم إضافة القسم بنجاح.");
        }

        [HttpPut("mainsection")]
        public async Task<ActionResult> Update([FromForm] UpdateMainSection model)
        {


            var result = await _mainSectionService.Update(model);

            if (!result)
                return NotFound(new ApiResponse(404, "القسم غير موجود أو حدث خطأ أثناء التعديل."));
          
            return Ok("تم تعديل القسم بنجاح.");
        }

        [HttpDelete("mainsection/{id}")]
        public async Task<ActionResult> Remove(int id)
        {
            var result = await _mainSectionService.Remove(id);

            if (!result)
                return NotFound(new ApiResponse(404, "القسم غير موجود."));

            return Ok( "تم حذف القسم بنجاح." );
        }

        [HttpPut("deliverymethod")]
        public async Task<ActionResult> UpdateDeliveryMethod(UpdateDeliveryMethodDto model)
        {
            var result = await _admindashboard.UpdateDeliveryMethod(model);

            if (!result)
                return NotFound(new ApiResponse(404, "طريقة التوصيل غير موجودة أو حدث خطأ أثناء التعديل."));

            return Ok("تم تعديل طريقة التوصيل بنجاح.");
        }


        [HttpPost("partner")]
        public async Task<ActionResult> AddPartner(AddPartnerAccountDto request)
        {

            var accountDto = new AddPartnerAccountDto
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password
            };

            var managerId = await _admindashboard.AddPartnerAccount(accountDto);

            if (managerId == null)
                return BadRequest(new ApiResponse(400, "فشل إنشاء الحساب، قد يكون الإيميل مستخدماً."));

            var partnerDto = new AddPartnerDto
            {
                Name = request.Name,
                ManagerId = managerId,
            };

            var result = await _partnerService.AddAsync(partnerDto);


            return Ok("تم إنشاء حساب التاجر.");
        }


        [HttpGet("partners")]
        public async Task<ActionResult<IReadOnlyList<PartnerResponseDto>>> GetAllPartners(string? search)
        {
            var partners = await _partnerService.GetAllAsync(search);

            

            return Ok(partners);
        }


        [HttpGet("partner/{id}")]
        public async Task<ActionResult<PartnerWithBranchesResponseDto>> GetPartnerWithBranches(int id)
        {
            var partner = await _partnerService.GetPartnerWithBranchesAsync(id);

            if (partner == null)
                return NotFound(new ApiResponse(404, "التاجر غير موجود."));

            

            return Ok(partner);
        }


        [HttpDelete("partner/{id}")]
        public async Task<ActionResult> RemovePartner(int id)
        {
            var result = await _partnerService.RemoveAsync(id);

            if (!result)
                return NotFound(new ApiResponse(404, "التاجر غير موجود أو حدث خطأ أثناء الحذف."));

            return Ok("تم إيقاف التاجر وجميع فروعه بنجاح.");
        }

        [HttpPost("Activepartner/{id}")]
        public async Task<ActionResult> Activepartner(int id)
        {
            var result = await _partnerService.UpdatePartner(id);

            if (!result)
                return NotFound(new ApiResponse(404, "التاجر غير موجود أو حدث خطأ أثناء التعطيل"));

            return Ok("تم تنشيط التاجر وجميع فروعه بنجاح.");
        }
        [HttpPut("{partnerId}/settle")]
        public async Task<ActionResult> SettlePartnerAccount(int partnerId)
        {
            var result = await _admindashboard.SettlePartnerAccountAsync(partnerId);

            if (!result)
                return BadRequest(new ApiResponse(400, "حدث خطأ أثناء تسوية الحساب، قد يكون الشريك غير موجود."));

            return Ok(new ApiResponse(200, "تمت تسوية حساب الشريك وتصفير المحفظة بنجاح."));
        }

        [HttpGet("usersgrowth")]
        public async Task<ActionResult<YearlyGrowthReportDto>> GetUsersGrowth([FromQuery] int year = 2026)
        {
            var result = await _admindashboard.GetUsersGrowthAsync(year);
            return Ok(result);
        }

        [HttpGet("itemsgrowth")]
        public async Task<ActionResult<YearlyGrowthReportDto>> GetItemsGrowth([FromQuery] int year = 2026)
        {
            var result = await _admindashboard.GetItemsbuyGrowthAsync(year);
            return Ok(result);
        }

        [HttpGet("profitgrowth")]
        public async Task<ActionResult<YearlyGrowthReportDto>> GetProfitGrowth([FromQuery] int year = 2026)
        {
            var result = await _admindashboard.GetProfitGrowthAsync(year);
            return Ok(result);
        }
    }
}
