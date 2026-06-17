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
    public interface IBranchDashboardService
    {
        Task<int?> GetBranchIdByManagerAsync(string managerId);

        Task<IReadOnlyList<ItemDashbordDto>> GetAllItemsAsync(int branchId, string? searchName);
        Task<ItemDashbordDto?> GetItemByIdAsync(int id);
        Task<bool> AddItemAsync(int branchId, ItemCreateDto itemDto);
        Task<bool> UpdateItemAsync(int id, ItemUpdateDto itemDto);
        Task<bool> DeleteItemAsync(int id);

        Task<IReadOnlyList<CategoryDto>> GetCategoriesByBranchAsync(int branchId);
        Task<bool> AddCategoryAsync(int branchId, string Name);
        Task<bool> UpdateCategoryAsync(CategoryDto categoryDto);
        Task<bool> DeleteCategoryAsync(int id);

        Task<IReadOnlyList<Order>> GetOrdersFilteredAsync(int branchId, OrderStatus? status);
        Task<bool> ChangeOrderStatusAsync(int orderId, OrderStatus newStatus);

        Task<BranchReportDto> GetBranchReportAsync(int branchId, ReportTimeFilter timeFilter);
        Task<YearlyGrowthReportDto> GetProfitGrowthAsync(int branchid, int year);
        Task<YearlyGrowthReportDto> GetItemsbuyGrowthAsync(int branchid, int year);
    }
}
