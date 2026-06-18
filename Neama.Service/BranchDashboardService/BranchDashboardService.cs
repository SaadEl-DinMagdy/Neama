using Neama.Core;
using Neama.Core.Entities;
using Neama.Core.Entities.Order_Aggregate;
using Neama.Core.Services.Contract;
using Neama.Core.Specifications;
using Neama.Core.Specifications.BranchDashBordSpecification;
using Neama.Core.Specifications.OrderSpecifications;
using Shared.Dtos;
using Shared.shareEnumsAndEntitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Service.BranchDashboardService
{
    public class BranchDashboardService : IBranchDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAttachmentService _attachmentService;

        public BranchDashboardService(IUnitOfWork unitOfWork, IAttachmentService attachmentService)
        {
            _unitOfWork = unitOfWork;
            _attachmentService = attachmentService;
        }

        public async Task<int?> GetBranchIdByManagerAsync(string managerId)
        {
            var spec = new BaseSpecifications<Branch>(b => b.ManagerId == managerId);
            var branch = await _unitOfWork.Repository<Branch>().GetWithSpecAsync(spec);
            return branch?.Id;
        }

        public async Task<IReadOnlyList<ItemDashbordDto>> GetAllItemsAsync(int branchId, string? searchName)
        {
            var spec = new ItemsByBranchSpecification(branchId, searchName);
            var items = await _unitOfWork.Repository<Item>().GetAllWithSpecAsync(spec);

            return items.Select(i => new ItemDashbordDto
            {
                Id = i.Id,
                Name = i.Name,
                Description = i.Description,
                OriginalPrice = i.OriginalPrice,
                DiscountPrice = i.DiscountPrice,
                StockQuantity = i.StockQuantity,
                ExpiryDate = i.ExpiryDate,
                CategoryName = i.Category?.Name,
                ImageUrl = i.ImageURL
            }).ToList();
        }

        public async Task<ItemDashbordDto?> GetItemByIdAsync(int id)
        {
            var spec = new ItemsByBranchSpecification(id);
            var item = await _unitOfWork.Repository<Item>().GetWithSpecAsync(spec);
            if (item == null) return null;

            return new ItemDashbordDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                OriginalPrice = item.OriginalPrice,
                DiscountPrice = item.DiscountPrice,
                StockQuantity = item.StockQuantity,
                ExpiryDate = item.ExpiryDate,
                CategoryName = item.Category?.Name,
                ImageUrl = item.ImageURL
            };
        }

        public async Task<bool> AddItemAsync(int branchId, ItemCreateDto itemDto)
        {
            var item = new Item
            {
                Name = itemDto.Name,
                Description = itemDto.Description,
                OriginalPrice = itemDto.OriginalPrice,
                DiscountPrice = itemDto.DiscountPrice,
                StockQuantity = itemDto.StockQuantity,
                ExpiryDate = itemDto.ExpiryDate,
                BranchId = branchId,
                CategoryId = itemDto.CategoryId,
                IsActive = true,
                CreationDate = DateOnly.FromDateTime(DateTime.UtcNow)
            };

            if (itemDto.Image != null)
            {
                item.ImageURL = await _attachmentService.ImageUrl(itemDto.Image);
            }

            await _unitOfWork.Repository<Item>().AddAsync(item);
            return await _unitOfWork.CompleteAsync() > 0;
        }

        public async Task<bool> UpdateItemAsync(int id, ItemUpdateDto itemDto)
        {
            var item = await _unitOfWork.Repository<Item>().GetAsync(id);
            if (item == null) return false;

            item.Name = itemDto.Name;
            item.Description = itemDto.Description;
            item.OriginalPrice = itemDto.OriginalPrice;
            item.DiscountPrice = itemDto.DiscountPrice;
            item.StockQuantity = itemDto.StockQuantity;
            item.ExpiryDate = itemDto.ExpiryDate;
            item.CategoryId = itemDto.CategoryId;

            if (itemDto.Image != null)
            {
                if (!string.IsNullOrEmpty(item.ImageURL))
                {
                    await _attachmentService.DeleteImageByUrl(new List<string> { item.ImageURL });
                }
                item.ImageURL = await _attachmentService.ImageUrl(itemDto.Image);
            }

            _unitOfWork.Repository<Item>().Update(item);
            return await _unitOfWork.CompleteAsync() > 0;
        }

        public async Task<bool> DeleteItemAsync(int id)
        {
            var item = await _unitOfWork.Repository<Item>().GetAsync(id);
            if (item == null) return false;

            item.IsActive = false;

     
             if (!string.IsNullOrEmpty(item.ImageURL))
             {
                 await _attachmentService.DeleteImageByUrl(new List<string> { item.ImageURL });
             }

            item.ImageURL = null;
            _unitOfWork.Repository<Item>().Update(item);

            return await _unitOfWork.CompleteAsync() > 0;
        }


        public async Task<IReadOnlyList<CategoryDto>> GetCategoriesByBranchAsync(int branchId)
        {
            var spec = new CategoriesByBranchSpecification(branchId);
            var categories = await _unitOfWork.Repository<Category>().GetAllWithSpecAsync(spec);

            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            }).ToList();
        }

        public async Task<bool> AddCategoryAsync(int branchId, string Name)
        {
            var category = new Category
            {
                Name = Name,
                BranchId = branchId,
                CreationDate = DateOnly.FromDateTime(DateTime.UtcNow)
            };
            await _unitOfWork.Repository<Category>().AddAsync(category);
            return await _unitOfWork.CompleteAsync() > 0;
        }

        public async Task<bool> UpdateCategoryAsync( CategoryDto categoryDto)
        {
            var category = await _unitOfWork.Repository<Category>().GetAsync(categoryDto.Id);
            if (category == null) return false;

            category.Name = categoryDto.Name;
            _unitOfWork.Repository<Category>().Update(category);
            return await _unitOfWork.CompleteAsync() > 0;
        }

        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var category = await _unitOfWork.Repository<Category>().GetAsync(id);
            if (category == null) return false;

            _unitOfWork.Repository<Category>().Delete(category);
            return await _unitOfWork.CompleteAsync() > 0;
        }

        public async Task<IReadOnlyList<Order>> GetOrdersFilteredAsync(int branchId, OrderStatus? status)
        {
            var spec = new OrdersByBranchAndStatusSpec(branchId, status);
            return await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);
        }

        public async Task<bool> ChangeOrderStatusAsync(int orderId, OrderStatus newStatus)
        {
            var order = await _unitOfWork.Repository<Order>().GetAsync(orderId);
            if (order == null) return false;

            order.Status = newStatus;
            _unitOfWork.Repository<Order>().Update(order);
            return await _unitOfWork.CompleteAsync() > 0;
        }

        public async Task<BranchReportDto> GetBranchReportAsync(int branchId, ReportTimeFilter timeFilter)
        {
            var orderSpec = new OrdersForReportSpecification(timeFilter, null, branchId, null);
            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(orderSpec);

            var completedOrders = orders.Where(o => o.Status == OrderStatus.Delivered).ToList();

            var totalSales = completedOrders.Sum(o => o.SubTotal);
            var totalProfits = totalSales * 0.90m;

            var itemSpec = new BaseSpecifications<Item>(i => i.BranchId == branchId && i.IsActive);
            var activeItemsCount = await _unitOfWork.Repository<Item>().GetCountAsync(itemSpec);

            return new BranchReportDto
            {
                TotalSales = totalSales,
                TotalProfits = totalProfits,
                CompletedOrdersCount = completedOrders.Count,
                DisplayedItemsCount = activeItemsCount
            };
        }

        public async Task<YearlyGrowthReportDto> GetProfitGrowthAsync(int branshid, int year)
        {
            var spec = new OrdersForReportSpecification(null, branchId: branshid, specificYear: year);
            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);

            var ItembuyGrowthList = orders
                .Where(u => u.OrderDate.Year == year && u.BranchId == branshid)
                .GroupBy(u => u.OrderDate.Month)
                .Select(g => new
                {
                    Month = g.Key,

                    Count = (int)(g.Sum(order => order.SubTotal) * 0.90m)
                })
                .ToList();
            var report = new YearlyGrowthReportDto
            {
                Jan = ItembuyGrowthList.FirstOrDefault(x => x.Month == 1)?.Count ?? 0,
                Feb = ItembuyGrowthList.FirstOrDefault(x => x.Month == 2)?.Count ?? 0,
                Mar = ItembuyGrowthList.FirstOrDefault(x => x.Month == 3)?.Count ?? 0,
                Apr = ItembuyGrowthList.FirstOrDefault(x => x.Month == 4)?.Count ?? 0,
                May = ItembuyGrowthList.FirstOrDefault(x => x.Month == 5)?.Count ?? 0,
                Jun = ItembuyGrowthList.FirstOrDefault(x => x.Month == 6)?.Count ?? 0,
                Jul = ItembuyGrowthList.FirstOrDefault(x => x.Month == 7)?.Count ?? 0,
                Aug = ItembuyGrowthList.FirstOrDefault(x => x.Month == 8)?.Count ?? 0,
                Sep = ItembuyGrowthList.FirstOrDefault(x => x.Month == 9)?.Count ?? 0,
                Oct = ItembuyGrowthList.FirstOrDefault(x => x.Month == 10)?.Count ?? 0,
                Nov = ItembuyGrowthList.FirstOrDefault(x => x.Month == 11)?.Count ?? 0,
                Dec = ItembuyGrowthList.FirstOrDefault(x => x.Month == 12)?.Count ?? 0
            };

            return report;
        }

        public async Task<YearlyGrowthReportDto> GetItemsbuyGrowthAsync(int branshid, int year)
        {
            var spec = new OrdersForReportSpecification(null, branchId: branshid, specificYear: year);
            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);

            var ItembuyGrowthList = orders
                .Where(u => u.OrderDate.Year == year && u.BranchId == branshid)
                .GroupBy(u => u.OrderDate.Month)
                .Select(g => new
                {
                    Month = g.Key,

                    Count = g.Sum(order => order.Items.Sum(item => item.Quantity))
                })
                .ToList();
            var report = new YearlyGrowthReportDto
            {
                Jan = ItembuyGrowthList.FirstOrDefault(x => x.Month == 1)?.Count ?? 0,
                Feb = ItembuyGrowthList.FirstOrDefault(x => x.Month == 2)?.Count ?? 0,
                Mar = ItembuyGrowthList.FirstOrDefault(x => x.Month == 3)?.Count ?? 0,
                Apr = ItembuyGrowthList.FirstOrDefault(x => x.Month == 4)?.Count ?? 0,
                May = ItembuyGrowthList.FirstOrDefault(x => x.Month == 5)?.Count ?? 0,
                Jun = ItembuyGrowthList.FirstOrDefault(x => x.Month == 6)?.Count ?? 0,
                Jul = ItembuyGrowthList.FirstOrDefault(x => x.Month == 7)?.Count ?? 0,
                Aug = ItembuyGrowthList.FirstOrDefault(x => x.Month == 8)?.Count ?? 0,
                Sep = ItembuyGrowthList.FirstOrDefault(x => x.Month == 9)?.Count ?? 0,
                Oct = ItembuyGrowthList.FirstOrDefault(x => x.Month == 10)?.Count ?? 0,
                Nov = ItembuyGrowthList.FirstOrDefault(x => x.Month == 11)?.Count ?? 0,
                Dec = ItembuyGrowthList.FirstOrDefault(x => x.Month == 12)?.Count ?? 0
            };

            return report;
        }

    }
}
