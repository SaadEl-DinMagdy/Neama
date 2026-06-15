using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Neama.Api.Errors;
using Neama.Api.Helper;
using Neama.Core;
using Neama.Core.Entities;
using Neama.Core.Services.Contract;
using Neama.Core.Specifications.BranchSpecification;
using Shared.Dtos;

namespace Neama.Api.Controllers
{

    [Authorize]
    public class BranchController : BaseApiController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBranchService _branchService;

        public BranchController(IUnitOfWork unitOfWork, IBranchService branchService)
        {
            _unitOfWork = unitOfWork;
            _branchService = branchService;
        }

        [HttpGet("NearBranches")]
        public async Task<ActionResult<Pagination<AllBranchDto>>> GetNearBranches([FromQuery] BranchParams param)
        {
            var Data = await _branchService.GetAllSpecAsync(param);
            var spec = new BranchWithSearchAndSortCountSpec(param);
            var count = await _unitOfWork.Repository<Branch>().GetCountAsync(spec);

            return new Pagination<AllBranchDto>(param.PageIndex, param.PageSize, count, Data);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BranchDto>> GetBranch(int id)
        {
            var branch = await _branchService.GetAsync(id);

            if (branch == null)
            {
                return NotFound(new ApiResponse(404));
            }
            return Ok(branch);
        }
    }
}
