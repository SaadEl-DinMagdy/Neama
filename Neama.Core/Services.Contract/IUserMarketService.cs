using Neama.Core.Entities;
using Neama.Core.Specifications.UserMarketSpecification;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Core.Services.Contract
{
    public interface IUserMarketService
    {
        Task<IReadOnlyList<ReturnUserProductDto>> GetAllMyProductSpecAsync(string userId ,string? Search);
        Task<IReadOnlyList<ReturnUserProductDto>> GetUserMarketSpecAsync(MarketParam param);
        Task<ReturnUserProductDto?> GetMyProductSpecAsync(string userId , int Id);
        Task<bool> AddAsync(AddUserProduct userProduct , string userId);
        Task<ReturnUserProductDto?> Update(UserProductDto userProduct);
        Task<bool> Remove(int id);
    }
}
