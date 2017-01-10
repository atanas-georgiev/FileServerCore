namespace AvgIdentity.Extensions
{
    using Avg.Data.Models;
    using AvgIdentity.Exceptions;
    using AvgIdentity.Managers;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class MigrationExtensions
    {
        public static void AddAvgIdentityMigration<TContext, TUser>(this IApplicationBuilder app, IServiceScopeFactory scopeFactory, IConfiguration configuration)
                where TUser : AvgIdentityUser
                where TContext : IdentityDbContext<TUser>
        {
            using (var serviceScope = scopeFactory.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<TContext>();
                var userRoleManager = serviceScope.ServiceProvider.GetService<IUserRoleManager<TUser>>();
                
                try
                {
                    if (!context.Roles.Any())
                    {
                        SeedRoles(userRoleManager, configuration);
                        SeedUsers(userRoleManager, configuration);
                    }
                }
                catch
                {
                    context.Database.Migrate();

                    if (!context.Roles.Any())
                    {
                        SeedRoles(userRoleManager, configuration);
                        SeedUsers(userRoleManager, configuration);
                    }
                }
            }
        }

        private static void SeedUsers<TUser>(IUserRoleManager<TUser> userRoleManager, IConfiguration configuration) where TUser : AvgIdentityUser
        {
            var roles = new List<string>();

            try
            {
                roles = configuration
                    .GetSection("AvgIdentity")
                    .GetSection("InitialData")
                    .GetSection("Users")
                    .GetChildren()
                    .Select(c => c.Value).ToList();                
            }
            catch
            {
                throw new AvgIdentityConfigurationException("AvgIdentity InitialData error");
            }

            userRoleManager.AddRoles(roles);
        }

        private static void SeedRoles<TUser>(IUserRoleManager<TUser> userRoleManager, IConfiguration configuration) where TUser : AvgIdentityUser
        {
            throw new NotImplementedException();
        }
    }
}
