using Microsoft.AspNetCore.Identity;
using Models.Enums;

namespace Data.Context
{
    public static class Seeder
    {
        public static async Task SeedAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
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
            var identityUser = new IdentityUser() { Email = "aji@gmail.com", UserName = "Admin" };
            var result = await userManager.CreateAsync(identityUser, "@Streets2023");
            if (result.Succeeded)
                await userManager.AddToRoleAsync(identityUser, Roles.Admin.ToString());
            else
                throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
        }
    }
}
