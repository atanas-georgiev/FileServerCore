using FileServerCore.Web.Areas.Shared.Controllers;
using Microsoft.AspNetCore.Mvc;
using Avg.Data;
using Avg.Data.Models;
using AvgIdentity.Managers;
using FileServerCore.Web.Resources;
using Microsoft.Extensions.Localization;

namespace FileServerCore.Web.Areas.Admin.Controllers
{
    // [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class UsersController : BaseController
    {
        public UsersController(IUserRoleManager<AvgIdentityUser, AvgDbContext> userRoleManager, IStringLocalizer<Labels> localizedLabels, IStringLocalizer<ErrorMessages> localizedErrorMessages) : base(userRoleManager, localizedLabels, localizedErrorMessages)
        {
        }

        public IActionResult Index()
        {
            return this.View();
        }
    }
}
