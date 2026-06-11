using Neama.Core.Entities;
using Shared.Dtos;
using Shared.shareEnumsAndEntitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Services.Contract
{
    public interface IPartnerDashboardService
    {
        Task<Partner?> GetPartnerByManagerIdAsync(string managerId);
        Task<bool> AddBranchAsync(int partnerId, AddBranchDto model);
        Task<bool> RemoveBranchAsync(int branchId, int partnerId);
        Task<bool> UpdatePartnerAsync(int partnerId, UpdatePartnerDto model);
        Task<IReadOnlyList<AllBranchResponseDto>> GetAllBranchesAsync(int partnerId);
        Task<BranchDetailsDto?> GetBranchDetailsAsync(int branchId, int partnerId, ReportTimeFilter filter, int? year);
        Task<PartnerReportDto> GetPartnerReportsAsync(int partnerId, ReportTimeFilter filter, int? year);
    }
}
