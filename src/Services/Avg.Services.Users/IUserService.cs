namespace Avg.Services.Users
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Avg.Data.Models;

    using Microsoft.AspNetCore.Identity;

    public interface IUserService
    {
        Task<AvgUser> AddAsync(AvgUser user, string password);

        Task<AvgUser> AddAsync(string email, string firstName, string lastName, string password, byte[] avatar);

        Task AddExternalLoginInfoAsync(AvgUser user, ExternalLoginInfo info);

        Task DeleteAsync(string id);

        Task DeleteAsync(AvgUser user);

        IQueryable<AvgUser> GetAll();

        AvgUser GetByEmail(string email);

        AvgUser GetById(string id);

        Task UpdateAsync(AvgUser user);

        void AddRoles(string[] roles);

        bool RemoveRoles(string[] roles);

        IQueryable<string> GetAllRoles();
    }
}