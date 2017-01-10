namespace FileServerCore.Web.Infrastructure.Middlewares
{
    using System.Linq;

    using Avg.Data;
    using Avg.Data.Models;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using AvgIdentity.Managers;

    public static class MigrationMiddleware
    {
        public static void AddAutomaticMigration(
            this IApplicationBuilder app,
            IUserRoleManager<AvgIdentityUser> userService,
            IServiceScopeFactory scopeFactory,
            IConfigurationRoot configuration)
        {
            using (var serviceScope = scopeFactory.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AvgDbContext>();
                try
                {
                    if (!context.Roles.Any())
                    {
                        SeedRoles(userService, configuration);
                        SeedUsers(userService, configuration);
                    }
                }
                catch
                {
                    context.Database.Migrate();

                    if (!context.Roles.Any())
                    {
                        SeedRoles(userService, configuration);
                        SeedUsers(userService, configuration);
                    }
                }
            }
        }

        private static void SeedRoles(IUserRoleManager<AvgIdentityUser> userService, IConfiguration configuration)
        {
            var roles =
                configuration.GetSection("Security")
                    .GetSection("SupportedRoles")
                    .GetChildren()
                    .Select(c => c.Value)
                    .ToArray();
            userService.AddRoles(roles);
        }

        private static void SeedUsers(IUserRoleManager<AvgIdentityUser> userService, IConfiguration configuration)
        {
            var initialUser = configuration.GetSection("Security").GetSection("InitialUser").GetChildren();
            var user = new AvgIdentityUser()
                           {
                               Email = initialUser.First(x => x.Key == "Email").Value,
                               FirstName = initialUser.First(x => x.Key == "FirstName").Value,
                               LastName = initialUser.First(x => x.Key == "LastName").Value
                           };

            userService.AddUserAsync(user, initialUser.First(x => x.Key == "Password").Value, initialUser.First(x => x.Key == "Role").Value);
        }
    }
}