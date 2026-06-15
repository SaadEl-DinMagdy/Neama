using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neama.Api.Errors;

namespace Neama.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class errorsController : ControllerBase
    {
        [HttpGet]
        public ActionResult Error(int statusCode)
        {
            if (statusCode == 401)
                return Unauthorized(new ApiResponse(401));
            else if (statusCode == 404)
                return NotFound(new ApiResponse(404));
            else
                return StatusCode(statusCode);
        }
    }
}
