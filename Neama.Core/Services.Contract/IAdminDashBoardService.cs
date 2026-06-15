using Microsoft.AspNetCore.Identity;
using Neama.Core.Entities.Order_Aggregate;
using Shared.Dtos;
using Shared.shareEnumsAndEntitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Services.Contract
{
    public interface IAdminDashBoardService
    {
        Task<IReadOnlyList<UserInfoDto>> GetAllUserAsync();
        Task<bool> UpdateDeliveryMethod(UpdateDeliveryMethodDto model);

        Task<string?> AddPartnerAccount(AddPartnerAccountDto model);

        Task<DashboardReportDto> GetDashboardReportsAsync(ReportTimeFilter filter, int? specificYear = null);

        Task<bool> SettlePartnerAccountAsync(int partnerId);
    }
}
