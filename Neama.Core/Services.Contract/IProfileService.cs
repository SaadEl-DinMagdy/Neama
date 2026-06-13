using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Services.Contract
{
    public interface IProfileService
    {

        Task<UserInfoDto?> GetUserInfoAsync(string email);

        Task<UserImpactStatsDto> GetUserImpactStatsAsync(string email);
    }
}
