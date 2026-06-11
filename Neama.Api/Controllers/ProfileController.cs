using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neama.Api.Errors;
using Neama.Core.Services.Contract;
using Shared.Dtos;
using System.Security.Claims;

namespace Neama.Api.Controllers
{
    [Authorize] 
    public class ProfileController : BaseApiController
    {
        private readonly IProfileService _profileService;

        public ProfileController(IProfileService profileService)
        {
            _profileService = profileService;
        }


        [HttpGet("info")]
        public async Task<ActionResult<UserInfoDto>> GetUserInfo()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var userInfo = await _profileService.GetUserInfoAsync(email);

            return Ok(userInfo);
        }


        [HttpGet("impactstats")]
        public async Task<ActionResult<UserImpactStatsDto>> GetUserStatus()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var stats = await _profileService.GetUserImpactStatsAsync(email);

            return Ok(stats);
        }
    }
}
