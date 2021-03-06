﻿namespace AvgIdentity.Managers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;
    using Avg.Data.Models;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

    using Remotion.Linq.Utilities;

    public class UserRoleManager<TUser, TContext> : IUserRoleManager<TUser, TContext>
        where TUser : AvgIdentityUser, new()
        where TContext : IdentityDbContext<TUser>
    {
        protected const string InitialPassword = "changeme";

        protected UserManager<TUser> UserManager { get; set; }

        protected SignInManager<TUser> SignInManager { get; set; }

        protected TContext Context { get; set; }

        public UserRoleManager(UserManager<TUser> userManager, SignInManager<TUser> signInManager, TContext context)
        {
            this.UserManager = userManager;
            this.SignInManager = signInManager;
            this.Context = context;
        }

        #region Users
        public virtual async Task<TUser> AddUserAsync(TUser user, string password, string role = null)
        {
            if (user != null)
            {
                user.CreatedOn = DateTime.UtcNow;
                user.UserName = user.Email;

                var hasher = new PasswordHasher<TUser>();
                if (user.PasswordAnswerHash != null)
                {
                    user.PasswordAnswerHash = hasher.HashPassword(user, user.PasswordAnswerHash);
                }

                var result = await this.UserManager.CreateAsync(user, password ?? InitialPassword);

                if (result.Succeeded)
                {
                    if (role != null)
                    {
                        await this.AddUserInRoleAsync(user, role);
                    }

                    return user;
                }
            }

            return null;
        }

        public async Task<TUser> AddUserAsync(string email, string password, string question = null, string answer = null, string firstName = null, string lastName = null, string role = null)
        {
            var user = new TUser { Email = email, PasswordQuestion = question, PasswordAnswerHash = answer, FirstName = firstName, LastName = lastName };
            return await this.AddUserAsync(user, password, role);
        }

        public async Task<bool> DeleteUserAsync(TUser user)
        {
            var result = await this.UserManager.DeleteAsync(user);
            return result.Succeeded;
        }

        public async Task<bool> DeleteUserAsync(string email)
        {
            var user = this.GetUser(email);

            if (user == null)
            {
                return false;
            }

            return await this.DeleteUserAsync(user);
        }

        public async Task<bool> UpdateUserAsync(TUser user)
        {
            var result = await this.UserManager.UpdateAsync(user);
            return result.Succeeded;
        }

        public IQueryable<TUser> GetAllUsers() => this.UserManager.Users;

        public TUser GetUser(string email) => this.UserManager.Users.FirstOrDefault(u => u.Email == email);

        public async Task<bool> ChangePasswordAsync(TUser user, string oldPassword, string newPassword)
        {
            var result = await this.UserManager.ChangePasswordAsync(user, oldPassword, newPassword);
            return result.Succeeded;
        }

        public async Task<bool> ResetPasswordAsync(TUser user, string passwordAnswer, string newPassword)
        {
            var hasher = new PasswordHasher<TUser>();
            var isPasswordCorrect = hasher.VerifyHashedPassword(user, user.PasswordAnswerHash, passwordAnswer);

            if (isPasswordCorrect == PasswordVerificationResult.Success)
            {
                var token = await this.UserManager.GeneratePasswordResetTokenAsync(user);
                var result = await this.UserManager.ResetPasswordAsync(user, token, newPassword);

                return result.Succeeded;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> CheckPasswordAsync(TUser user, string password)
        {
            return await this.UserManager.CheckPasswordAsync(user, password);
        }

        #endregion

        #region Roles

        public async Task<bool> AddRoleAsync(string role)
        {
            await this.Context.Roles.AddAsync(new IdentityRole(role));
            var result = await this.Context.SaveChangesAsync();
            return (result != 0);            
        }

        public async Task<bool> AddRolesAsync(IEnumerable<string> roles)
        {
            await this.Context.Roles.AddRangeAsync(roles.Select(r => new IdentityRole(r)));
            var result = await this.Context.SaveChangesAsync();
            return (result != 0);
        }

        public async Task<bool> AddUserInRoleAsync(TUser user, string role)
        {
            if (!(await this.UserManager.IsInRoleAsync(user, role)))
            {


                await this.Context.UserRoles.AddAsync(new IdentityUserRole<string>()
                {
                    RoleId = this.Context.Roles.First(x => x.Name == role).Id,
                    UserId = user.Id
                });
                this.Context.SaveChanges();
            }

            return true;
        }

        public async Task<bool> RemoveRoleAsync(string role)
        {
            if (this.GetAllUsersinRole(role).Any())
            {
                return false;
            }
            else
            {
                var roleDb = this.Context.Roles.FirstOrDefault(r => r.Name == role);
                this.Context.Roles.Remove(roleDb);
                var result = await this.Context.SaveChangesAsync();
                return (result != 0);
            }
        }

        public async Task<bool> RemoveRolesAsync(IEnumerable<string> roles)
        {            
            if (roles.All(role => this.GetAllUsersinRole(role).Any()))
            {
                return false;
            }
            else
            {
                var rolesDb = this.Context.Roles.Where(x => roles.Contains(x.Name));
                this.Context.Roles.RemoveRange(rolesDb);
                var result = await this.Context.SaveChangesAsync();
                return (result != 0);
            }
        }

        public async Task<bool> RemoveUserFromRoleAsync(TUser user, string role)
        {
            return false;
        }

        public IQueryable<string> GetAllRoles()
        {            
            return this.Context.Roles.Select(r => r.Name);
        }

        #endregion


        public async Task AddUserExternalLoginInfoAsync(TUser user, ExternalLoginInfo info)
        {
            await this.UserManager.AddLoginAsync(user, info);
        }



        public IQueryable<TUser> GetAllUsersinRole(string role)
        {
            var roleDb = this.Context.Roles.FirstOrDefault(x => x.Name == role);

            if (roleDb != null)
            {
                return this.Context.Users.Where(x => x.Roles.Any(r => r.RoleId == roleDb.Id));
            }

            return new List<TUser>().AsQueryable();                       
        }


        
        public async Task<bool> SignInAsync(TUser user, string password = null)
        {
            if (!await this.SignInManager.CanSignInAsync(user))
            {
                return false;
            }

            if (password == null)
            {
                await this.SignInManager.SignInAsync(user, true);
                return true;
            }
            else
            {
                var result = await this.SignInManager.PasswordSignInAsync(user, password, true, false);

                if (result.Succeeded)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<bool> SignInAsync(string email, string password = null)
        {
            var user = this.GetUser(email);

            if (user == null)
            {
                return false;
            }

            return await this.SignInAsync(user, password);
        }
    }
}