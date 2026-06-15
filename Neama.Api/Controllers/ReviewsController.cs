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
    public class ReviewsController : BaseApiController
    {
        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }


        [HttpPost]
        public async Task<ActionResult> AddReview([FromForm] ReviewCreateDto model)
        {

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new ApiResponse(401, "عفواً، يجب تسجيل الدخول لإضافة تقييم."));

            var result = await _reviewService.AddReviewAsync(userId, model);

            if (!result)
                return BadRequest(new ApiResponse(400, "حدث خطأ أثناء إضافة التقييم، قد يكون الفرع غير موجود."));

            return Ok(new ApiResponse(200, "تمت إضافة تقييمك بنجاح، شكراً لمشاركتك!"));
        }


        [HttpGet("stories")]
        public async Task<ActionResult<IReadOnlyList<ReviewDto>>> GetTop100StoryReviews()
        {
            var reviews = await _reviewService.GetTop100StoryReviewsAsync();
            return Ok(reviews);
        }


        [HttpGet("branch/{branchId}/Reviews")]
        public async Task<ActionResult<BranchReviewsResponseDto>> GetAllReviewsWithReport(int branchId)
        {
            var reportData = await _reviewService.GetAllReviewsWithReportAsync(branchId);
            return Ok(reportData);
        }
    }
}
