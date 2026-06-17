using Neama.Core;
using Neama.Core.Entities;
using Neama.Core.Services.Contract;
using Neama.Core.Specifications;
using Neama.Core.Specifications.BranchSpecification;
using NetTopologySuite.Operation.Distance;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Service.BranchService
{
    public class BranchService : IBranchService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BranchService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IReadOnlyList<AllBranchDto>> GetAllSpecAsync(BranchParams branchparams)
        {
            var spec =  new BranchWithSearchAndSortSpec(branchparams);
            var Data = await _unitOfWork.Repository<Branch>().GetAllWithSpecAsync(spec);

            var userlocation = LocationHelper.GetUserPoint(branchparams.Longitude, branchparams.Latitude);
            IReadOnlyList<AllBranchDto> Result =  Data
                .Select( B => new AllBranchDto
                {
                    Id = B.Id,
                    BranchName = B.BranchName,
                    AverageRating = B.AverageRating,
                    OpeningTime = B.OpeningTime,
                    ClosingTime = B.ClosingTime,
                    distance = Math.Round(B.Location.Distance(userlocation) * 111.32, 2),
                    ItemavilableCount =B.Items.Sum( i=> i.StockQuantity),
                    Cover_URL = B.Partner.Cover_URL,
                    Logo_URL = B.Partner.Logo_URL


                }).ToList();

            return Result;
            
        }

        public async Task<BranchDto?> GetAsync(int branchId)
        {
            var spec = new BranchwithPartner(branchId);

            var Branch = await _unitOfWork.Repository<Branch>().GetWithSpecAsync(spec);

            if(Branch == null)
            {
                return null;
            }
            return new BranchDto()
            {
                Id = Branch.Id,
                BranchName = Branch.BranchName,
                AverageRating = Branch.AverageRating,
                ReviewCount = Branch.ReviewCount,
                OpeningTime = Branch.OpeningTime,
                ClosingTime = Branch.ClosingTime,
                Cover_URL = Branch.Partner.Cover_URL,
                Logo_URL = Branch.Partner.Logo_URL,
                Longitude = Branch.Longitude,
                Latitude = Branch.Latitude

            };
        }
        public Task<bool> Add(Branch branch)
        {
            throw new NotImplementedException();
        }
        public Task<bool> Update(Branch branch)
        {
            throw new NotImplementedException();
        }
        public Task<bool> Remove(Branch branch)
        {
            throw new NotImplementedException();
        }

        
    }
}
