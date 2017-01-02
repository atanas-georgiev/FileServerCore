namespace FileServerCore.Web.Areas.FileManager.Controllers
{
    using Avg.Services.Users;

    using FileServerCore.Web.Areas.Shared.Controllers;
    using FileServerCore.Web.Resources;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;

    // [Authorize(Roles = "Admin")]
    [Area("FileManager")]
    public class HomeController : BaseController
    {
        public HomeController(
            IUserService userService,
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