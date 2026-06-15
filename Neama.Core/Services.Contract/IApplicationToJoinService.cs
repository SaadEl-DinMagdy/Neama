using Neama.Core.Entities;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Services.Contract
{
    public interface IApplicationToJoinService
    {
        Task<bool> CreateApplicationAsync(CreateApplicationToJoinDto application);
        Task<IReadOnlyList<ApplicationsToJoin>> GetAllApplicationsAsync(bool? ContactWasMade);
        Task<bool> MarkAsContactedAsync(int id);
    }
}
