using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Services.Contract
{
    public interface ICharityService
    {
        Task<IReadOnlyList<AllCharityDto>> GetAllAsync();
        Task<CharityDto?> GetByIdAsync(int id);
    }
}
