using Data.Context;
using Microsoft.EntityFrameworkCore;

namespace Clones_Api.Extensions
{
    public static class ConfigureDbContextExtension
    {
        private static string GetRenderConnectionString()
        {
            return Environment.GetEnvironmentVariable("DatabaseUrl")!;
        }

        public static void AddDbContextConfigurations(this IServiceCollection services, IWebHostEnvironment env, IConfiguration config)
        {
            services.AddDbContextPool<ClonesDbContext>(options =>
            {
                string connStr;

                if (env.IsProduction())
                {
                    connStr = GetRenderConnectionString();
                }
                else
                {
                    connStr = config.GetConnectionString("DefaultConnection");
                }
                options.UseNpgsql(connStr);
            });
        }
    }
}
