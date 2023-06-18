using Application;
using Application.Interfaces;
using Clones_Api.Extensions;
using Clones_Api.Middleware;
using Data.Context;
using Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Models;
using Models.Mapping;

namespace Clones_Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            // Add services to the container.
            builder.Services.AddDbContextConfigurations(builder.Environment, configuration);
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddTransient<ICredentialService, CredentialService>();
            builder.Services.AddTransient<IRepository, CredentialRepository>();
            builder.Services.AddTransient<IUserService, UserService>();
            builder.Services.AddAutoMapper(typeof(MappingProfile));
            builder.Services.ConfigureIdentity();
            builder.Services.ConfigureAuthentication(configuration);
            builder.Services.ConfigureSwaggerGen();

            // Configure application pipelines
            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ClonesDbContext>();
                context.Database.Migrate();
                if (context.Database.CanConnect())
                {
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                    Seeder.SeedAsync(userManager, roleManager, configuration).GetAwaiter().GetResult();
                }
            }

            if (!configuration.GetValue<bool>("DisableSwagger"))
            {
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Clones_Api v1"));
            }

            app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            app.UseHttpsRedirection();
            app.UseMiddleware<ExceptionMiddleware>();
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.UseStaticFiles();
            app.MapControllers();
            app.Run();
        }
    }
}