using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neama.Api.Errors;
using Neama.Core.Entities;
using Neama.Core.Services.Contract;
using Shared.Dtos;
using System.Security.Claims;
using System.Timers;

namespace Neama.Api.Controllers
{
    [Authorize]
    public class FavoriteController : BaseApiController
    {
        private readonly IFavoriteService _favoriteService;
        public FavoriteController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        private string GetCurrentUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier);

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<AllFavoriteDto>>> GetMyFavorites(string? Search)
        {
            var userId = GetCurrentUserId();

           
            var favorites = await _favoriteService.GetAllSpecAsync(userId , Search);

            return Ok(favorites);
        }

        [HttpPost]
        public async Task<ActionResult<FavoriteDto>> AddFavorite(int branchId)
        {
            var userId = GetCurrentUserId();

            var favorite = await _favoriteService.AddAsync(userId , branchId);
            if (favorite==null) return BadRequest(new ApiResponse(400,"الفرع مفضل بالفعل"));

            return Ok(favorite);
        }

        [HttpDelete]
        public async Task<ActionResult> RemoveFavorite(int Id)
        {
            var userId = GetCurrentUserId();

            var check = await _favoriteService.Remove(Id);

            if (!check) return BadRequest(new ApiResponse(400));


            return Ok("تم حذف من المقضله");
        }
    }
}
