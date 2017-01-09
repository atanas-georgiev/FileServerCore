using Avg.Services.Users;
using FileServerCore.Web.Infrastructure.Helpers;
using System.Linq;
using System.Threading.Tasks;

namespace FileServerCore.Web.Areas.Shared.Models
{
    public static class MappingFunctions
    {
        public static int GetUsersCountInRole(string role)
        {            
            var userService = (UserService)ServicesHelper.ServiceProvider.GetService(typeof(IUserService));
            return userService.GetAllUsersinRole(role).Count();
        }
    }
}
