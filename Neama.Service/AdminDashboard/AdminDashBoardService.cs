using Microsoft.AspNetCore.Identity;
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
    }
}
