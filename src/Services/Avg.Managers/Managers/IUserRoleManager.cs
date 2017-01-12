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

        Task<bool> ChangePasswordAsync(TUser user, string oldPassword, string newPassword);

        Task<bool> ResetPasswordAsync(TUser user, string passwordAnswer, string newPassword);

        Task<bool> CheckPasswordAsync(TUser user, string password);

        IQueryable<TUser> GetAllUsers();

        TUser GetUser(string email);

        #endregion

        #region Roles

        Task<bool> AddRoleAsync(string role);

        Task<bool> AddRolesAsync(IEnumerable<string> roles);

        Task<bool> AddUserInRoleAsync(TUser user, string role);

        Task<bool> RemoveRoleAsync(string role);

        Task<bool> RemoveRolesAsync(IEnumerable<string> roles);

        Task<bool> RemoveUserFromRoleAsync(TUser user, string role);

        IQueryable<string> GetAllRoles();

        IQueryable<TUser> GetAllUsersinRole(string role);        

        #endregion

        #region Sign

        Task AddUserExternalLoginInfoAsync(TUser user, ExternalLoginInfo info);

        Task<bool> SignInAsync(TUser user, string password = null);

        Task<bool> SignInAsync(string email, string password = null);

        #endregion
    }
}