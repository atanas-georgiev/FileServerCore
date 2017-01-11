namespace FileServerCore.Web.Areas.Shared.Models
{
    using Avg.Data;
    using Avg.Data.Models;
    using AvgIdentity.Managers;
    using FileServerCore.Web.Infrastructure.Helpers;
    using System.Linq;

    public static class MappingFunctions
    {
        public static int GetUsersCountInRole(string role)
        {            
            var userService = (UserRoleManager<AvgIdentityUser, AvgDbContext>)ServicesHelper.ServiceProvider.GetService(typeof(IUserRoleManager<AvgIdentityUser, AvgDbContext>));
            return userService.GetAllUsersinRole(role).Count();
        }
    }
}
