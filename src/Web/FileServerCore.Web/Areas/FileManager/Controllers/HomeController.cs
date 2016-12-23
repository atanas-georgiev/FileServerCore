using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FileServerCore.Data;
using FileServerCore.Data.Models;
using FileServerCore.Services.Users;
using FileServerCore.Web.Areas.Shared.Controllers;
using FileServerCore.Web.Resources;
using Microsoft.Extensions.Localization;

namespace FileServerCore.Web.Areas.FileManager.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Area("FileManager")]
    public class HomeController : BaseController
    {
        public HomeController(IUserService userService, IStringLocalizer<Labels> localizedLabels, IStringLocalizer<ErrorMessages> localizedErrorMessages, UserManager<User> userManager, SignInManager<User> signInManager, FileServerCoreDbContext dbContext) : base(userService, localizedLabels, localizedErrorMessages, userManager, signInManager, dbContext)
        {
        }

        public IActionResult Index()
        {
            return this.View();
        }
    }
}
