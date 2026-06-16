using Neama.Core.Entities;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Services.Contract
{
    public interface IPartnerService
    {
        Task<bool> AddAsync(AddPartnerDto model);
        Task<bool> UpdatePartner(int id);
        Task<IReadOnlyList<PartnerResponseDto>> GetAllAsync(string? search);
        Task<PartnerWithBranchesResponseDto?> GetPartnerWithBranchesAsync(int id);
        Task<bool> RemoveAsync(int id);
    }
}
