using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neama.Api.Errors;
using Neama.Core.Services.Contract;
using Shared.Dtos;

namespace Neama.Api.Controllers
{

    public class JoinUsController : BaseApiController
    {
        private readonly IApplicationToJoinService _appservice;

        public JoinUsController(IApplicationToJoinService appservice)
        {
            _appservice = appservice;
        }
        [HttpPost("ApplicationsToJoinPartner")]
        public async Task<ActionResult> SubmitApplication(CreateApplicationToJoinDto model)
        {

            var result = await _appservice.CreateApplicationAsync(model);

            if (!result)
                return BadRequest(new ApiResponse(400, "حدث خطأ أثناء إرسال الطلب. يرجى المحاولة لاحقاً."));

            return Ok("تم إرسال الطلب بنجاح، سيتم التواصل معك قريباً.");
        }
    }
}
