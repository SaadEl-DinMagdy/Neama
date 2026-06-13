using Neama.Core;
using Neama.Core.Entities;
using Neama.Core.Services.Contract;
using Neama.Core.Specifications.ReviewsByBranchSpecification;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Service.ReviewService
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAttachmentService _attachmentService;

        public ReviewService(IUnitOfWork unitOfWork, IAttachmentService attachmentService)
        {
            _unitOfWork = unitOfWork;
            _attachmentService = attachmentService;
        }

        public async Task<bool> AddReviewAsync(string userId, ReviewCreateDto dto)
        {
            var branch = await _unitOfWork.Repository<Branch>().GetAsync(dto.BranchId);
            if (branch == null) return false;

            var review = new Review
            {
                BranchId = dto.BranchId,
                UserId = userId,
                Comment = dto.Comment,
                Rating = dto.Rating,
                ImageUrl = await _attachmentService.ImageUrl(dto.Image),
                CreationDate = DateTimeOffset.UtcNow
            };

            await _unitOfWork.Repository<Review>().AddAsync(review);

            var currentTotalStars = branch.AverageRating * branch.ReviewCount;
            branch.ReviewCount += 1;
            branch.AverageRating = (currentTotalStars + dto.Rating) / branch.ReviewCount;

            _unitOfWork.Repository<Branch>().Update(branch);

            return await _unitOfWork.CompleteAsync() > 0;
        }

        public async Task<IReadOnlyList<ReviewDto>> GetTop100StoryReviewsAsync()
        {
            var spec = new TopStoryReviewsSpecification(100);
            var reviews = await _unitOfWork.Repository<Review>().GetAllWithSpecAsync(spec);

            return reviews.Select(r => new ReviewDto
            {
                Id = r.Id,
                Comment = r.Comment,
                Rating = r.Rating,
                ImageUrl = r.ImageUrl,
                UserName = r.User?.DisplayName ,
                BranchId = r.BranchId,
                CreationDate = r.CreationDate
            }).ToList();
        }

        public async Task<BranchReviewsResponseDto> GetAllReviewsWithReportAsync(int branchId)
        {
            var spec = new ReviewsByBranchSpecification(branchId);
            var reviews = await _unitOfWork.Repository<Review>().GetAllWithSpecAsync(spec);

            var reviewsList = reviews.Select(r => new ReviewDto
            {
                Id = r.Id,
                Comment = r.Comment,
                Rating = r.Rating,
                ImageUrl = r.ImageUrl,
                UserName = r.User?.DisplayName ?? "مستخدم نِعمة",
                BranchId = r.BranchId, 
                CreationDate = r.CreationDate
            }).ToList();

            var totalReviews = reviews.Count;
            var averageRating = totalReviews > 0 ? (decimal)reviews.Average(r => r.Rating) : 0;

            var report = new RatingReportDto
            {
                TotalReviews = totalReviews,
                AverageRating = Math.Round(averageRating, 1),
                FiveStarCount = reviews.Count(r => r.Rating == 5),
                FourStarCount = reviews.Count(r => r.Rating == 4),
                ThreeStarCount = reviews.Count(r => r.Rating == 3),
                TwoStarCount = reviews.Count(r => r.Rating == 2),
                OneStarCount = reviews.Count(r => r.Rating == 1)
            };

            return new BranchReviewsResponseDto
            {
                Report = report,
                Reviews = reviewsList
            };
        }
    }
}
