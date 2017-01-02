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

        public async Task<AvgUser> AddAsync(AvgUser user, string password)
        {
            if (user != null)
            {
                user.CreatedOn = DateTime.Now;
                user.UserName = user.Email;
                var result = await this.userManager.CreateAsync(user, password ?? InitialPassword);

                if (result.Succeeded)
                {
                    return user;
                }
            }

            return null;
        }

        public async Task<AvgUser> AddAsync(string email, string firstName, string lastName, string password, byte[] avatar)
        {
            var user = new AvgUser { Email = email, FirstName = firstName, LastName = lastName, Avatar = avatar };

            return await this.AddAsync(user, password);
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

        public void AddRoles(IList<string> roles)
        {
            this.context.Roles.AddRange(roles.Select(r => new IdentityRole(r)));
            this.context.SaveChanges();
        }
    }
}