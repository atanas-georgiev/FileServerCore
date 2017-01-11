namespace FileServerCore.Web.Areas.Account.Controllers
{
    // using Kendo.Mvc.Extensions;
    using System.Linq;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;

    using FileServerCore.Web.Areas.Account.Models;
    using FileServerCore.Web.Areas.Shared.Controllers;
    using FileServerCore.Web.Resources;
    using Microsoft.Extensions.Localization;
    using AvgIdentity.Managers;
    using Avg.Data.Models;
    using Avg.Data;

    [Authorize]
    [Area("Account")]
    public class ManageController : BaseController
    {
        public ManageController(IUserRoleManager<AvgIdentityUser, AvgDbContext> userRoleManager, IStringLocalizer<Labels> localizedLabels, IStringLocalizer<ErrorMessages> localizedErrorMessages) : base(userRoleManager, localizedLabels, localizedErrorMessages)
        {
        }

        public IActionResult Index()
        {
            var model = new AccountManageViewModel()
                            {
                                Email = this.UserProfile.Email,
                                FirstName = this.UserProfile.FirstName,
                                LastName = this.UserProfile.LastName
                            };

            return this.View(model);
        }

        [HttpPost]
        public IActionResult Index(AccountManageViewModel model)
        {
            var user = this.UserRoleManager.GetAllUsers().FirstOrDefault(x => x.UserName == model.Email);

            if (user != null && user.UserName != this.UserProfile.UserName)
            {
                this.ModelState.AddModelError("Email", LocalizedErrorMessages["UsernameExist"]);
            }
            else if (this.ModelState.IsValid)
            {
                this.UserProfile.UserName = model.Email;
                this.UserProfile.Email = model.Email;
                this.UserProfile.FirstName = model.FirstName;
                this.UserProfile.LastName = model.LastName;

                // this.UserRoleManager.Update(this.UserProfile);
                return this.RedirectToAction("Index", "Home", new { area = string.Empty });
            }

            return this.View(model);
        }
    }
}