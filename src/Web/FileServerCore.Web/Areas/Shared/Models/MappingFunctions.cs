namespace FileServerCore.Web.Areas.Shared.Models
{
    using Avg.Data.Models;
    using AvgIdentity.Managers;
    using FileServerCore.Web.Infrastructure.Helpers;
    using System.Linq;

    public static class MappingFunctions
    {
        public static int GetUsersCountInRole(string role)
        {            
            var userService = (UserRoleManager)ServicesHelper.ServiceProvider.GetService(typeof(IUserRoleManager<AvgIdentityUser>));
            return userService.GetAllUsersinRole(role).Count();
        }
    }
}
