using Microsoft.AspNetCore.Identity;
using Neama.Core;
using Neama.Core.Entities;
using Neama.Core.Entities.Order_Aggregate;
using Neama.Core.Services.Contract;
using Neama.Core.Specifications;
using Neama.Core.Specifications.BranchSpecification;
using Neama.Core.Specifications.OrderSpecifications;
using Neama.Core.Specifications.PartnerSpecifications;
using Shared;
using Shared.Dtos;
using Shared.shareEnumsAndEntitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Service.PartnerDashboardService
{
    public class PartnerDashboardService : IPartnerDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IAttachmentService _attachmentService;

        public PartnerDashboardService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, IAttachmentService attachmentService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _attachmentService = attachmentService; 
        }

        public async Task<Partner?> GetPartnerByManagerIdAsync(string managerId)
        {
            var spec = new BaseSpecifications<Partner>( p =>p.ManagerId == managerId );
            var partners = await _unitOfWork.Repository<Partner>().GetWithSpecAsync(spec);
            return partners;
        }

        public async Task<bool> AddBranchAsync(int partnerId, AddBranchDto model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null) return false;

            var user = new AppUser
            {
                DisplayName = model.BranchName,
                Email = model.Email,
                UserName = model.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return false;

            await _userManager.AddToRoleAsync(user, AppRoles.Branch);

            var branch = new Branch
            {
                BranchName = model.BranchName,
                BranchPhone = model.BranchPhone,
                Latitude = model.Latitude,
                Longitude = model.Longitude,
                OpeningTime = model.OpeningTime,
                ClosingTime = model.ClosingTime,
                AverageRating = 5,
                ReviewCount = 1,
                Location = LocationHelper.GetUserPoint(model.Longitude, model.Latitude),
                Is_Active = true,
                CreationDate = DateOnly.FromDateTime(DateTime.UtcNow),
                ManagerId = user.Id,
                PartnerId = partnerId,
            };

            await _unitOfWork.Repository<Branch>().AddAsync(branch);
            var Result = await _unitOfWork.CompleteAsync();

            if (Result <= 0)
            {
                await _userManager.DeleteAsync(user);
                return false;
            }

            return true;
        }

        public async Task<bool> RemoveBranchAsync(int branchId, int partnerId)
        {
            var branch = await _unitOfWork.Repository<Branch>().GetAsync(branchId);
            if (branch == null || branch.PartnerId != partnerId) return false;

            branch.Is_Active = false;
            _unitOfWork.Repository<Branch>().Update(branch);
            return await _unitOfWork.CompleteAsync() > 0;
        }


        public async Task<bool> UpdatePartnerAsync(int partnerId, UpdatePartnerDto model)
        {
            var partner = await _unitOfWork.Repository<Partner>().GetAsync(partnerId);
            if (partner == null) return false;

            partner.Name = model.Name;
            partner.MainSectionId = model.mainSectionId;

            if (model.Logo != null)
            {
                if (!string.IsNullOrEmpty(partner.Logo_URL))
                {
                    await _attachmentService.DeleteImageByUrl(new List<string> { partner.Logo_URL });
                }

                partner.Logo_URL = await _attachmentService.ImageUrl(model.Logo);
            }

            if (model.Cover != null)
            {
                if (!string.IsNullOrEmpty(partner.Cover_URL))
                {
                    await _attachmentService.DeleteImageByUrl(new List<string> { partner.Cover_URL });
                }
                partner.Cover_URL = await _attachmentService.ImageUrl(model.Cover);
            }

            _unitOfWork.Repository<Partner>().Update(partner);
            return await _unitOfWork.CompleteAsync() > 0;
        }

        public async Task<IReadOnlyList<AllBranchResponseDto>> GetAllBranchesAsync(int partnerId)
        {
            var spec = new BaseSpecifications<Branch>(b => b.PartnerId == partnerId);
            var branches = await _unitOfWork.Repository<Branch>().GetAllWithSpecAsync(spec);

            var data = branches.Select(b => new AllBranchResponseDto
            {
                Id = b.Id,
                BranchName = b.BranchName,
                BranchPhone = b.BranchPhone,
            }).ToList();
            return data ;
        }

        public async Task<BranchDetailsDto?> GetBranchDetailsAsync(int branchId, int partnerId, ReportTimeFilter filter, int? year)
        {
            var branch = await _unitOfWork.Repository<Branch>().GetAsync(branchId);
            if (branch == null || branch.PartnerId != partnerId) return null;

            string? email = null;
            if (branch.ManagerId != null)
            {
                var manager = await _userManager.FindByIdAsync(branch.ManagerId);
                email = manager?.Email;
            }

            var spec = new OrdersForReportSpecification(filter, branchId: branchId, specificYear: year);
            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);

            var totalSales = orders.Sum(o => o.SubTotal);

            return new BranchDetailsDto
            {
                Id = branch.Id,
                BranchName = branch.BranchName,
                BranchPhone = branch.BranchPhone,
                Is_Active = branch.Is_Active,
                Email = email,
                OpeningTime = branch.OpeningTime,
                ClosingTime = branch.ClosingTime,
                Latitude = branch.Latitude,
                Longitude = branch.Longitude,
                SoldItems = orders.Sum(o => o.SavedMealsCount),
                BranchSales = totalSales,
                BranchNetProfit = totalSales * 0.90m
            };
        }

        public async Task<PartnerReportDto> GetPartnerReportsAsync(int partnerId, ReportTimeFilter filter, int? year)
        {
            var branches = await GetAllBranchesAsync(partnerId);
            var partner = await _unitOfWork.Repository<Partner>().GetAsync(partnerId); 

            var spec = new OrdersForReportSpecification(filter, partnerId: partnerId, specificYear: year);
            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);

            var totalSales = orders.Sum(o => o.SubTotal);

            return new PartnerReportDto
            {
                TotalBranches = branches.Count,
                TotalSoldItems = orders.Sum(o => o.SavedMealsCount),
                TotalSales = totalSales,
                NetProfit = totalSales * 0.90m,
                WalletBalance = partner.WalledBalance
                
            };
        }
        public async Task<YearlyGrowthReportDto> GetProfitGrowthAsync(int partnerid,int year)
        {
            var spec = new OrdersForReportSpecification(null , partnerId: partnerid, specificYear: year);
            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);

            var ItembuyGrowthList = orders
                .Where(u => u.OrderDate.Year == year)
                .GroupBy(u => u.OrderDate.Month)
                .Select(g => new
                {
                    Month = g.Key,

                    Count = (int)(g.Sum(order => order.SubTotal)*0.90m)
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

        public async Task<YearlyGrowthReportDto> GetItemsbuyGrowthAsync(int partnerid,int year)
        {
            var spec = new OrdersForReportSpecification(null, partnerId: partnerid, specificYear: year);
            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);

            var ItembuyGrowthList = orders
                .Where(u => u.OrderDate.Year == year)
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

        public async Task<PartnerResponseDto?> GetPartnerDetilsAsync(int partnerId)
        {
            var spec =  new PartnerWithBranchesSpecification(partnerId);
            var data = await _unitOfWork.Repository<Partner>().GetAllWithSpecAsync(spec);

            if (data.Count == 0)
            {
                return null;
            }

            var result = new PartnerResponseDto
            {
                Id = data[0].Id,
                Name = data[0].Name,
                WalledBalance = data[0].WalledBalance,
                Is_Active = data[0].Is_Active,
                Logo_URL = data[0].Logo_URL,
                Cover_URL = data[0].Cover_URL,
                CreationDate = data[0].CreationDate,
                ManagerEmail = data[0].Manager != null ? data[0].Manager.Email : null,
                MainSectionName = data[0].MainSection != null ? data[0].MainSection.Name : null
            };

            return result;
        }
    }
}
