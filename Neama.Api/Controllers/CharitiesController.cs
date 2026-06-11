using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neama.Api.Errors;
using Neama.Core.Services.Contract;
using Shared.Dtos;

namespace Neama.Api.Controllers
{
    [Authorize]
    public class CharitiesController : BaseApiController
    {
        private readonly ICharityService _charityService;

        public CharitiesController(ICharityService charityService)
        {
            _charityService = charityService;
        }


        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AllCharityDto>>> GetAll()
        {
            var charities = await _charityService.GetAllAsync();

            return Ok(charities);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<CharityDto>> GetById(int id)
        {
            var charity = await _charityService.GetByIdAsync(id);

            if (charity == null)
            {

                return NotFound(new ApiResponse(404));
            }

            return Ok(charity);
        }
    }
}
