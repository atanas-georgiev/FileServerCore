namespace FileServerCore.Web.Areas.Account.Controllers
{
    using System.Linq;
    using System.Threading.Tasks;

    using Avg.Data;
    using Avg.Data.Models;

    using AvgIdentity.Managers;

    using FileServerCore.Web.Areas.Account.Models;
    using FileServerCore.Web.Areas.Shared.Controllers;
    using FileServerCore.Web.Resources;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;

    [Authorize]
    [Area("Account")]
    public class ManageController : BaseController
    {
        public ManageController(
            IUserRoleManager<AvgIdentityUser, AvgDbContext> userRoleManager,
            IStringLocalizer<Labels> localizedLabels,
            IStringLocalizer<ErrorMessages> localizedErrorMessages)
            : base(userRoleManager, localizedLabels, localizedErrorMessages)
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
        public async Task<IActionResult> Index(AccountManageViewModel model)
        {
            var user = this.UserRoleManager.GetUser(model.Email);

            if (user != null && user.Email != this.UserProfile.Email)
            {
                this.ModelState.AddModelError("Email", this.LocalizedErrorMessages["UsernameExist"]);
            }
            else if (this.ModelState.IsValid)
            {
                this.UserProfile.UserName = model.Email;
                this.UserProfile.Email = model.Email;
                this.UserProfile.FirstName = model.FirstName;
                this.UserProfile.LastName = model.LastName;

                await this.UserRoleManager.UpdateUserAsync(this.UserProfile);
                return this.RedirectToAction("Index", "Home", new { area = string.Empty });
            }

            return this.View(model);
        }
    }
}