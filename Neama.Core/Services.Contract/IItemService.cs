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
    public interface IItemService
    {
        Task<IReadOnlyList<AllItemDto>> GetAllSpecAsync(int branchId , int? categoryId);
        Task<ItemDto?> GetItemAsync(int ItemId);
        Task<bool> Add(Item item);
        Task<bool> Update(Item item);
        Task<bool> Remove(Item item);
    }
}
