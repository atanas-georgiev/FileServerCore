namespace AvgIdentity.Managers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Avg.Data.Models;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

    public interface IUserRoleManager<TUser, TContext>
        where TUser : AvgIdentityUser, new()
        where TContext : IdentityDbContext<TUser>
    {

        #region Users

            Task<TUser> AddUserAsync(TUser user, string password, string role = null);

            Task<TUser> AddUserAsync(string email, string firstName, string lastName, string password, byte[] avatar, string role = null);

            Task AddUserExternalLoginInfoAsync(TUser user, ExternalLoginInfo info);

            Task DeleteUserAsync(string id);

            Task DeleteUserAsync(TUser user);

            Task UpdateUserAsync(TUser user);

            IQueryable<TUser> GetAllUsers();

            TUser GetUserByEmail(string email);

            TUser GetUserById(string id);            

        #endregion

        #region Roles

            void AddRoles(IEnumerable<string> roles);

            bool RemoveRoles(IEnumerable<string> roles);

            IQueryable<string> GetAllRoles();

            IQueryable<TUser> GetAllUsersinRole(string role);

            Task AddUserInRole(TUser user, string role);

        #endregion
    }
}