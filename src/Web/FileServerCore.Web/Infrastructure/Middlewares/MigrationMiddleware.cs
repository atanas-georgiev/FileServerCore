namespace FileServerCore.Web.Infrastructure.Middlewares
{
    using System.Linq;

    using Avg.Data;
    using Avg.Data.Models;
    using Avg.Services.Users;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class MigrationMiddleware
    {
        public static void AddAutomaticMigration(
            this IApplicationBuilder app,
            IUserService userService,
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

        private static void SeedRoles(IUserService userService, IConfiguration configuration)
        {
            var roles =
                configuration.GetSection("Security")
                    .GetSection("SupportedRoles")
                    .GetChildren()
                    .Select(c => c.Value)
                    .ToArray();
            userService.AddRoles(roles);
        }

        private static void SeedUsers(IUserService userService, IConfiguration configuration)
        {
            var initialUser = configuration.GetSection("Security").GetSection("InitialUser").GetChildren();
            var user = new AvgUser()
                           {
                               Email = initialUser.First(x => x.Key == "Email").Value,
                               FirstName = initialUser.First(x => x.Key == "FirstName").Value,
                               LastName = initialUser.First(x => x.Key == "LastName").Value
                           };

            userService.AddAsync(user, initialUser.First(x => x.Key == "Password").Value);
        }
    }
}