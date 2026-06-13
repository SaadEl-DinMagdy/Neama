using Neama.Core.Entities;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Services.Contract
{
    public interface IMainSectionService
    {
        Task<IReadOnlyList<AllMainSectionDto>> GetAllAsync();
        Task<bool> Add(AddMainSectionDto section); 
        Task<bool> Update(UpdateMainSection section);
        Task<bool> Remove(int id);
    }
}
