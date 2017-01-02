namespace FileServerCore.Web.Infrastructure.Middlewares
{
    using System;
    using System.Linq;

    using Avg.Data;
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

                        // SeedAdmin(context);
                    }
                }
                catch
                {
                    context.Database.Migrate();

                    if (!context.Roles.Any())
                    {
                        var roles = configuration.GetSection("Security").GetSection("SupportedRoles").GetChildren().Select(c => c.Value).ToList();
                        userService.AddRoles(roles);

                        // SeedAdmin(context);
                    }
                }
            }
        }

        private static void SeedRoles(IUserService userService, IConfiguration configuration)
        {
            
        }
    }
}