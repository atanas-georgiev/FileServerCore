namespace FileServerCore.Web.Areas.Admin.Controllers
{
    using Avg.Data.Models;
    using AvgIdentity.Managers;
    using FileServerCore.Web.Areas.Shared.Controllers;
    using FileServerCore.Web.Resources;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;

    // [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class RolesController : BaseController
    {
        public RolesController(
            IUserRoleManager<AvgIdentityUser> userService,
            IStringLocalizer<Labels> localizedLabels,
            IStringLocalizer<ErrorMessages> localizedErrorMessages)
            : base(userService, localizedLabels, localizedErrorMessages)
        {
        }

        public IActionResult Index()
        {
            return this.View();
        }
    }
}