namespace Avg.Services.Users
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Avg.Data;
    using Avg.Data.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

    public class UserService : IUserService
    {
        private const string InitialPassword = "changeme";

        private readonly UserManager<AvgUser> userManager;

        private readonly AvgDbContext context;

        public UserService(UserManager<AvgUser> userManager, AvgDbContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }

        public async Task<AvgUser> AddAsync(AvgUser user, string password, string role = null)
        {
            if (user != null)
            {
                user.CreatedOn = DateTime.Now;
                user.UserName = user.Email;
                var result = await this.userManager.CreateAsync(user, password ?? InitialPassword);

                if (result.Succeeded)
                {
                    if (role != null)
                    {
                        await this.AddUserInRole(user, role);
                    }

                    return user;
                }
            }

            return null;
        }

        public async Task<AvgUser> AddAsync(string email, string firstName, string lastName, string password, byte[] avatar, string role = null)
        {
            var user = new AvgUser { Email = email, FirstName = firstName, LastName = lastName, Avatar = avatar };

            return await this.AddAsync(user, password, role);
        }

        public async Task DeleteAsync(AvgUser user)
        {
            await this.userManager.DeleteAsync(user);
        }

        public async Task DeleteAsync(string id)
        {
            await this.userManager.DeleteAsync(this.GetById(id));
        }

        public IQueryable<AvgUser> GetAll()
        {
            return this.userManager.Users;
        }

        public AvgUser GetByEmail(string email)
        {
            return this.userManager.Users.FirstOrDefault(u => u.Email == email);
        }

        public AvgUser GetById(string id)
        {
            return this.userManager.Users.FirstOrDefault(u => u.Id == id);
        }

        public async Task UpdateAsync(AvgUser user)
        {
            await this.userManager.UpdateAsync(user);
        }

        public void AddRoles(string[] roles)
        {
            this.context.Roles.AddRange(roles.Select(r => new IdentityRole(r)));
            this.context.SaveChanges();
        }

        public async Task AddExternalLoginInfoAsync(AvgUser user, ExternalLoginInfo info)
        {
            await this.userManager.AddLoginAsync(user, info);
        }

        public bool RemoveRoles(string[] roles)
        {
            if (roles.All(role => this.GetAllUsersinRole(role).Count() <= 0))
            {
                var rolesDb = this.context.Roles.Where(x => roles.Contains(x.Name));

                this.context.RemoveRange(rolesDb);
                this.context.SaveChanges();
                return true;
            }

            return false;
        }

        public IQueryable<string> GetAllRoles()
        {
            return this.context.Roles.Select(r => r.Name);
        }

        public IQueryable<AvgUser> GetAllUsersinRole(string role)
        {
            var roleDb = this.context.Roles.FirstOrDefault(x => x.Name == role);

            if (roleDb != null)
            {
                return this.context.Users.Where(x => x.Roles.Any(r => r.RoleId == roleDb.Id));
            }

            return new List<AvgUser>().AsQueryable();

            var a = this.userManager.GetUsersInRoleAsync(role).Result;
            return this.userManager.GetUsersInRoleAsync(role).Result.AsQueryable();
        }

        public async Task AddUserInRole(AvgUser user, string role)
        {
            if (!(await userManager.IsInRoleAsync(user, role)))
            {
                this.context.UserRoles.Add(new IdentityUserRole<string>()
                {
                    RoleId = this.context.Roles.First(x => x.Name == role).Id,
                    UserId = user.Id
                });
                this.context.SaveChanges();
            }
        }
    }
}