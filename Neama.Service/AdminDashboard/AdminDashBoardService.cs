using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Neama.Core;
using Neama.Core.Entities;
using Neama.Core.Entities.Order_Aggregate;
using Neama.Core.Services.Contract;
using Neama.Core.Specifications;
using Neama.Core.Specifications.OrderSpecifications;
using Shared;
using Shared.Dtos;
using Shared.shareEnumsAndEntitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Service.AdminDashboard
{
    public class AdminDashBoardService : IAdminDashBoardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public AdminDashBoardService(IUnitOfWork unitOfWork , UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<DashboardReportDto> GetDashboardReportsAsync(ReportTimeFilter filter, int? specificYear)
        {
            var report = new DashboardReportDto();

            var users = await _userManager.GetUsersInRoleAsync(AppRoles.User);
            report.TotalUsers = users.Count;

            var partners = await _unitOfWork.Repository<Partner>().GetAllAsync();
            report.TotalPartners = partners.Count;


            var charities = await _unitOfWork.Repository<Charity>().GetAllAsync();
            report.TotalCharities = charities.Count;

            var speca = new BaseSpecifications<Item>(i => i.IsActive);
            var items = await _unitOfWork.Repository<Item>().GetAllWithSpecAsync(speca);
            report.TotalOfferedMeals = items.Sum(i => i.StockQuantity);


            var spec = new OrdersForReportSpecification(filter, specificYear);
            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);

            report.TotalSavedMeals = orders.Sum(order => order.Items.Sum(item => item.Quantity));

            var totalSales = orders.Sum(o => o.SubTotal);
            report.TotalProfits = totalSales * 0.10m;

            return report;
        }
        public async Task<string?> AddPartnerAccount(AddPartnerAccountDto model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
                return null;

            var user = new AppUser
            {
                DisplayName = model.Name,
                Email = model.Email,
                UserName = model.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
                return null;


            await _userManager.AddToRoleAsync(user, AppRoles.Partner);

            return user.Id;
        }

        public async Task<bool> UpdateDeliveryMethod(UpdateDeliveryMethodDto model)
        {
            var data = await _unitOfWork.Repository<DeliveryMethod>().GetAsync(model.Id);
            if(data == null)
            {
                return false;
            }
            data.ShortName = model.ShortName;
            data.Description = model.Description;
            data.Cost = model.Cost;
            data.DeliveryTime = model.DeliveryTime;

            _unitOfWork.Repository<DeliveryMethod>().Update(data);
            var result = await _unitOfWork.CompleteAsync();
            return result > 0;

        }

        public async Task<bool> SettlePartnerAccountAsync(int partnerId)
        {
            var partner = await _unitOfWork.Repository<Partner>().GetAsync(partnerId);
            if (partner == null) return false;

            partner.WalledBalance = 0;

            _unitOfWork.Repository<Partner>().Update(partner);
            return await _unitOfWork.CompleteAsync() > 0;
        }

        public async Task<IReadOnlyList<UserInfoDto>> GetAllUserAsync()
        {
            var Users = await _userManager.GetUsersInRoleAsync(AppRoles.User);

            IReadOnlyList<UserInfoDto> Result = Users
                .Where(U => U.EmailConfirmed)
                .Select(U => new UserInfoDto
                {
                    DisplayName = U.DisplayName,
                    Email = U.Email
                }).ToList();

            return Result;
        }

        public async Task<YearlyGrowthReportDto> GetUsersGrowthAsync(int year)
        {
            var usersGrowthList = await _userManager.Users
                .Where(u => u.CreatedAt != null && u.CreatedAt.Value.Year == year)
                .GroupBy(u => u.CreatedAt.Value.Month)
                .Select(g => new { Month = g.Key, Count = g.Count() })
                .ToListAsync();

            var report = new YearlyGrowthReportDto
            {
                Jan = usersGrowthList.FirstOrDefault(x => x.Month == 1)?.Count ?? 0,
                Feb = usersGrowthList.FirstOrDefault(x => x.Month == 2)?.Count ?? 0,
                Mar = usersGrowthList.FirstOrDefault(x => x.Month == 3)?.Count ?? 0,
                Apr = usersGrowthList.FirstOrDefault(x => x.Month == 4)?.Count ?? 0,
                May = usersGrowthList.FirstOrDefault(x => x.Month == 5)?.Count ?? 0,
                Jun = usersGrowthList.FirstOrDefault(x => x.Month == 6)?.Count ?? 0,
                Jul = usersGrowthList.FirstOrDefault(x => x.Month == 7)?.Count ?? 0,
                Aug = usersGrowthList.FirstOrDefault(x => x.Month == 8)?.Count ?? 0,
                Sep = usersGrowthList.FirstOrDefault(x => x.Month == 9)?.Count ?? 0,
                Oct = usersGrowthList.FirstOrDefault(x => x.Month == 10)?.Count ?? 0,
                Nov = usersGrowthList.FirstOrDefault(x => x.Month == 11)?.Count ?? 0,
                Dec = usersGrowthList.FirstOrDefault(x => x.Month == 12)?.Count ?? 0
            };

            return report;
        }

        public async Task<YearlyGrowthReportDto> GetProfitGrowthAsync(int year)
        {
            var spec = new OrdersForReportSpecification(null, year);
            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);

            var ItembuyGrowthList = orders
                .Where(u => u.OrderDate.Year == year)
                .GroupBy(u => u.OrderDate.Month)
                .Select(g => new
                {
                    Month = g.Key,

                    Count = (int)(g.Sum(order => order.SubTotal) * 0.10m)
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

        public async Task<YearlyGrowthReportDto> GetItemsbuyGrowthAsync(int year)
        {
            var spec = new OrdersForReportSpecification(null, year);
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
    }
}
