using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Models;
using Models.Enums;

namespace Data.Context
{
    public static class Seeder
    {
        public static async Task SeedAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            var roles = new List<string>() { Roles.Admin.ToString(), Roles.User.ToString() };

            foreach (var roleName in roles)
            {
                var role = await roleManager.FindByNameAsync(roleName);
                if (role == null)
                {
                    var response = await roleManager.CreateAsync(new IdentityRole(roleName));
                    if (!response.Succeeded)
                        throw new Exception(string.Join(Environment.NewLine, response.Errors.Select(e => e.Description)));
                }
            }
            var identityUser = await userManager.FindByEmailAsync(configuration["Admin:Email"]);
            if (identityUser == null)
            {
                var user = new User() { Email = configuration["Admin:Email"], UserName = configuration["Admin:Username"], TenantId = "0000", IsActive = true };
                var result = await userManager.CreateAsync(user, configuration["Admin:Password"]);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(user, Roles.Admin.ToString());
                else
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
            }
        }
    }
}
