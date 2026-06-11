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
    public interface IFavoriteService
    {
        Task<IReadOnlyList<AllFavoriteDto>> GetAllSpecAsync(string userId,string? Search);
        Task<FavoriteDto?> AddAsync(string userId, int branchId);
        Task<bool> Remove(int Id);
    }
}
