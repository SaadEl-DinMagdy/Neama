using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Services.Contract
{
    public interface IReviewService
    {
        Task<bool> AddReviewAsync(string userId, ReviewCreateDto dto);
        Task<IReadOnlyList<ReviewDto>> GetTop100StoryReviewsAsync(); 
        Task<BranchReviewsResponseDto> GetAllReviewsWithReportAsync(int branchId);
    }
}
