using Neama.Core.Entities;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Services.Contract
{
    public interface ICategoryService
    {
        Task<IReadOnlyList<CategoryDto>> GetBranchCategoriesAsync(int branchId);
        Task<bool> Add(Category category);
        Task<bool> Update(Category category);
        Task<bool> Remove(Category category);
    }
}
