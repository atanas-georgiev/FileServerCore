namespace FileServerCore.Web.Areas.Account.Controllers
{
    using System.IO;
    using System.Threading.Tasks;

    using Avg.Data.Models;

    using FileServerCore.Web.Areas.Account.Models;
    using FileServerCore.Web.Areas.Shared.Controllers;
    using FileServerCore.Web.Resources;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;
    using AvgIdentity.Managers;
    using Avg.Data;

    [Area("Account")]
    public class RegisterController : BaseController
    {
        private readonly SignInManager<AvgIdentityUser> signInManager;

        public RegisterController(
            IUserRoleManager<AvgIdentityUser, AvgDbContext> userService,
            IStringLocalizer<Labels> localizedLabels,
            IStringLocalizer<ErrorMessages> localizedErrorMessages,
            SignInManager<AvgIdentityUser> signInManager)
            : base(userService, localizedLabels, localizedErrorMessages)
        {
            this.signInManager = signInManager;
        }

        public IActionResult Index(string returnUrl)
        {
            return this.View(new AccountRegisterViewModel() { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Index(AccountRegisterViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                //// TODO: avatar
                var user = await this.UserRoleManager.AddUserAsync(
                               model.Email,
                               model.FirstName,
                               model.LastName,
                               model.Password,
                               null);

                if (user != null)
                {
                    //// TODO: Add to role
                    await this.signInManager.SignInAsync(user, false);
                    if (string.IsNullOrEmpty(model.ReturnUrl))
                    {
                        return this.RedirectToAction("Index", "Home", new { area = string.Empty });
                    }

                    return this.RedirectToLocal(model.ReturnUrl);
                }

                this.ModelState.AddModelError("Email", this.LocalizedErrorMessages["UsernameExist"]);
            }

            return this.View(model);
        }

        private static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}