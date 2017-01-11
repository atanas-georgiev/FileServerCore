namespace FileServerCore.Web.Areas.FileManager.Controllers
{
    using Avg.Data;
    using Avg.Data.Models;
    using AvgIdentity.Managers;
    using FileServerCore.Web.Areas.Shared.Controllers;
    using FileServerCore.Web.Resources;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;

    // [Authorize(Roles = "Admin")]
    [Area("FileManager")]
    public class HomeController : BaseController
    {
        public HomeController(IUserRoleManager<AvgIdentityUser, AvgDbContext> userRoleManager, IStringLocalizer<Labels> localizedLabels, IStringLocalizer<ErrorMessages> localizedErrorMessages) : base(userRoleManager, localizedLabels, localizedErrorMessages)
        {
        }

        public IActionResult Index()
        {
            return this.View();
        }
    }
}