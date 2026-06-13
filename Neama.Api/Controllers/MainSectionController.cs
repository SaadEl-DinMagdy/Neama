using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neama.Api.Errors;
using Neama.Core.Services.Contract;
using Shared.Dtos;

namespace Neama.Api.Controllers
{
    [Authorize]
    public class MainSectionController : BaseApiController
    {
        private readonly IMainSectionService _mainSectionService;

        public MainSectionController(IMainSectionService mainSectionService)
        {
            _mainSectionService = mainSectionService;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AllMainSectionDto>>> GetAllMainSectionAsync()
        { 
           var Sections = await _mainSectionService.GetAllAsync();
            return Ok(Sections);
        }
    }
}
