using Neama.Core;
using Neama.Core.Entities;
using Neama.Core.Services.Contract;
using Neama.Core.Specifications;
using Neama.Core.Specifications.FavoriteSpecification;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Service.FavoriteService
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBranchService _branchService;

        public FavoriteService(IUnitOfWork unitOfWork , IBranchService branchService)
        {
            _unitOfWork = unitOfWork;
            _branchService = branchService;
        }
        public async Task<IReadOnlyList<AllFavoriteDto>> GetAllSpecAsync(string userId , string? Search)
        {
            var spec =  new FavoriteSearchAndIncludeSpece(userId,Search);
            var Data = await _unitOfWork.Repository<Favorite>().GetAllWithSpecAsync(spec);

            IReadOnlyList<AllFavoriteDto> Result = Data
                .Select(f => new AllFavoriteDto()
                {
                    Id = f.Id,
                    BranchId = f.BranchId,
                    BranchName = f.Branch.BranchName,
                    AverageRating = f.Branch.AverageRating,
                    OpeningTime = f.Branch.OpeningTime,
                    ClosingTime = f.Branch.ClosingTime,
                    Cover_URL = f.Branch.Partner.Cover_URL,
                    Logo_URL = f.Branch.Partner.Logo_URL

                }).ToList();

            return Result;
                
        }
        public async Task<FavoriteDto?> AddAsync(string userId , int branchId)
        {
            var spec = new BaseSpecifications<Favorite>(f => f.UserId == userId && f.BranchId == branchId);
            var favourit = await _unitOfWork.Repository<Favorite>().GetWithSpecAsync(spec);
            var branch = await _branchService.GetAsync(branchId);
            if (branch == null || favourit!=null)
            {
                return null;
            }
            var favorite = new Favorite() { UserId = userId , BranchId=branchId };

            await _unitOfWork.Repository<Favorite>().AddAsync(favorite);
            await _unitOfWork.CompleteAsync();
             spec = new BaseSpecifications<Favorite>(f => f.UserId ==  userId && f.BranchId==branchId);
             favourit = await _unitOfWork.Repository<Favorite>().GetWithSpecAsync(spec);
            return new FavoriteDto() { Id = favorite.Id,UserId = userId , BranchId = branchId};

        }

        public async Task<bool> Remove(int Id)
        {
            var spec = new BaseSpecifications<Favorite>(f => f.Id==Id);
            var favorite = await _unitOfWork.Repository<Favorite>().GetWithSpecAsync(spec);

            if(favorite == null)
            {
                return false;
            }

             _unitOfWork.Repository<Favorite>().Delete(favorite);
             await _unitOfWork.CompleteAsync();
            return true;
        }

        
    }
}
