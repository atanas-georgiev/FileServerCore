namespace AvgIdentity.Managers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Avg.Data.Models;
    using System.Collections.Generic;
    using System.Globalization;

    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

    public interface IUserRoleManager<TUser, TContext>
        where TUser : AvgIdentityUser, new()
        where TContext : IdentityDbContext<TUser>
    {

        #region Users

            Task<TUser> AddUserAsync(TUser user, string password, string role = null);

            Task<TUser> AddUserAsync(string email, string password, string question = null, string answer = null, string firstName = null, string lastName = null, string role = null);

            Task<bool> DeleteUserAsync(TUser user);

            Task<bool> DeleteUserAsync(string email);

            Task<bool> UpdateUserAsync(TUser user);

            IQueryable<TUser> GetAllUsers();

            TUser GetUser(string email);

            Task<bool> ChangePasswordAsync(TUser user, string oldPassword, string newPassword);

            Task<bool> ResetPasswordAsync(TUser user, string passwordAnswer, string newPassword);

            Task<bool> CheckPasswordAsync(TUser user, string password);

        #endregion

        #region Roles

        void AddRoles(IEnumerable<string> roles);

            bool RemoveRoles(IEnumerable<string> roles);

            IQueryable<string> GetAllRoles();

            IQueryable<TUser> GetAllUsersinRole(string role);

            Task AddUserInRole(TUser user, string role);

        #endregion

        #region Sign
            Task AddUserExternalLoginInfoAsync(TUser user, ExternalLoginInfo info);

            Task<bool> SignInAsync(TUser user, string password = null);

            Task<bool> SignInAsync(string email, string password = null);

        #endregion
    }
}