namespace AvgIdentity.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Avg.Data;
    using Avg.Data.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

    public class UserRoleManager : IUserRoleManager<AvgIdentityUser>
    {
        private const string InitialPassword = "changeme";

        private readonly UserManager<AvgIdentityUser> userManager;

        private readonly AvgDbContext context;

        public UserRoleManager(UserManager<AvgIdentityUser> userManager, AvgDbContext context)
        {
            this.userManager = userManager;
            this.context = context;
        }

        public virtual async Task<AvgIdentityUser> AddUserAsync(AvgIdentityUser user, string password, string role = null)
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

        public async Task<AvgIdentityUser> AddUserAsync(string email, string firstName, string lastName, string password, byte[] avatar, string role = null)
        {
            var user = new AvgIdentityUser { Email = email, FirstName = firstName, LastName = lastName, Avatar = avatar };

            return await this.AddUserAsync(user, password, role);
        }

        public async Task DeleteUserAsync(AvgIdentityUser user)
        {
            await this.userManager.DeleteAsync(user);
        }

        public async Task DeleteUserAsync(string id)
        {
            await this.userManager.DeleteAsync(this.GetUserById(id));
        }

        public IQueryable<AvgIdentityUser> GetAllUsers()
        {
            return this.userManager.Users;
        }

        public AvgIdentityUser GetUserByEmail(string email)
        {
            return this.userManager.Users.FirstOrDefault(u => u.Email == email);
        }

        public AvgIdentityUser GetUserById(string id)
        {
            return this.userManager.Users.FirstOrDefault(u => u.Id == id);
        }

        public async Task UpdateUserAsync(AvgIdentityUser user)
        {
            await this.userManager.UpdateAsync(user);
        }

        public void AddRoles(IEnumerable<string> roles)
        {
            this.context.Roles.AddRange(roles.Select(r => new IdentityRole(r)));
            this.context.SaveChanges();
        }

        public async Task AddUserExternalLoginInfoAsync(AvgIdentityUser user, ExternalLoginInfo info)
        {
            await this.userManager.AddLoginAsync(user, info);
        }

        public bool RemoveRoles(IEnumerable<string> roles)
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

        public IQueryable<AvgIdentityUser> GetAllUsersinRole(string role)
        {
            var roleDb = this.context.Roles.FirstOrDefault(x => x.Name == role);

            if (roleDb != null)
            {
                return this.context.Users.Where(x => x.Roles.Any(r => r.RoleId == roleDb.Id));
            }

            return new List<AvgIdentityUser>().AsQueryable();                       
        }

        public async Task AddUserInRole(AvgIdentityUser user, string role)
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