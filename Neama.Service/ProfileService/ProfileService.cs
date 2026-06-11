using Microsoft.AspNetCore.Identity;
using Neama.Core;
using Neama.Core.Entities;
using Neama.Core.Entities.Order_Aggregate;
using Neama.Core.Services.Contract;
using Neama.Core.Specifications.OrderSpecifications;
using Shared.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Service.ProfileService
{
    public class ProfileService : IProfileService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;

        public ProfileService(UserManager<AppUser> userManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
        }

        public async Task<UserInfoDto?> GetUserInfoAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null) return null;

            return new UserInfoDto
            {
                DisplayName = user.DisplayName, 
                Email = user.Email
            };
        }


        public async Task<UserImpactStatsDto> GetUserImpactStatsAsync(string email)
        {
            var spec = new OrderSpec(email , true);
            var orders = await _unitOfWork.Repository<Order>().GetAllWithSpecAsync(spec);


            int totalSavedMeals = orders.Sum(o => o.SavedMealsCount);
            decimal totalSavedAmount = orders.Sum(o => o.TotalSavedAmount);

            return new UserImpactStatsDto
            {
                TotalSavedMeals = totalSavedMeals,
                TotalSavedAmount = totalSavedAmount
            };
        }
    }
}
