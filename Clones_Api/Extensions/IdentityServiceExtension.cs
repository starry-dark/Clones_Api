using Data.Context;
using Microsoft.AspNetCore.Identity;

namespace Clones_Api.Extensions
{
    public static class IdentityServiceExtension
    {
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                //options.User.RequireUniqueEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                options.Password.RequiredLength = 6;
                //options.SignIn.RequireConfirmedEmail = true;
            });
            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), services);
            builder.AddEntityFrameworkStores<ClonesDbContext>()
                .AddDefaultTokenProviders();
        }
    }
}
