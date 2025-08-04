using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;
using TravelPlannerAPI.Models;

namespace TravelPlannerAPI.Seed
{
    public static class AdminSeeder
    {
        public static async Task SeedAdminUserAsync(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<UserModel>>();
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole<int>>>();
            var logger = services.GetService<ILoggerFactory>()?.CreateLogger("AdminSeeder");

            const string adminEmail = "admin@gmail.com";
            const string adminPassword = "Admin@1234";
            const string adminRole = "Admin";

            // Create Role if not exists
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole<int>(adminRole));
                if (!roleResult.Succeeded)
                {
                    logger?.LogError("Failed to create Admin role: {Errors}",
                        string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                    return;
                }
            }

            // Check if admin user exists
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new UserModel
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    Name = "Admin"
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                    logger?.LogInformation("Admin user created successfully.");
                }
                else
                {
                    logger?.LogError("Failed to create admin user: {Errors}",
                        string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            else
            {
                logger?.LogInformation("Admin user already exists.");
            }
        }
    }
}
