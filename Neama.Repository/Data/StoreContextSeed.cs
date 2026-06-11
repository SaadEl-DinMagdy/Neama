using Microsoft.AspNetCore.Identity;
using Neama.Core.Entities;
using Neama.Core.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neama.Repository.Data
{
    public static class StoreContextSeed
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
                await roleManager.CreateAsync(new IdentityRole("Admin"));

            if (!await roleManager.RoleExistsAsync("Partner"))
                await roleManager.CreateAsync(new IdentityRole("Partner"));

            if (!await roleManager.RoleExistsAsync("Branch"))
                await roleManager.CreateAsync(new IdentityRole("Branch"));

            if (!await roleManager.RoleExistsAsync("User"))
                await roleManager.CreateAsync(new IdentityRole("User"));
        }
        public static async Task SeedAdminUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any(u => u.Email == "saadeldin.magdy1@gmail.com"))
            {
                var adminUser = new AppUser
                {
                    DisplayName = "SuperAdmin", 
                    UserName = "saadeldin.magdy1@gmail.com",
                    Email = "saadeldin.magdy1@gmail.com",
                    EmailConfirmed = true 
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
        public static async Task SeedDeliveryMethodsAsync(StoreContext context)
        {
            if (!context.Set<DeliveryMethod>().Any())
            {
                var deliveryMethods = new List<DeliveryMethod>
                {
                    new DeliveryMethod(
                        shortName: "توصيل",
                        description: "توصيل الطلب إلى العنوان المحدد",
                        cost: 20m, 
                        deliveryTime: "30-45 دقيقة"
                    ),
                    new DeliveryMethod(
                        shortName: "استلام من الفرع",
                        description: "استلام الطلب شخصياً من مقر الفرع",
                        cost: 0m, 
                        deliveryTime: "جاهز للاستلام خلال 15 دقيقة"
                    )
                };

                await context.Set<DeliveryMethod>().AddRangeAsync(deliveryMethods);
                await context.SaveChangesAsync();
            }
        }

    }
}
