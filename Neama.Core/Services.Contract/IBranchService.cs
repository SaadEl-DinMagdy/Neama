using Neama.Core.Entities;
using Neama.Core.Specifications.BranchSpecification;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Services.Contract
{
    public interface IBranchService
    {
        Task<IReadOnlyList<AllBranchDto>> GetAllSpecAsync(BranchParams branchparams);
        Task<BranchDto?> GetAsync(int branchId);
        Task<bool> Add(Branch branch);
        Task<bool> Update(Branch branch);
        Task<bool> Remove(Branch branch);
    }
}
