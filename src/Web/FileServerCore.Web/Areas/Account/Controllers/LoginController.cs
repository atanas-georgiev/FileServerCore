namespace FileServerCore.Web.Areas.Account.Controllers
{
    using System;
    using System.Linq;
    using System.Security.Claims;
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
    public class LoginController : BaseController
    {
        private readonly SignInManager<AvgIdentityUser> signInManager;

        public LoginController(
            SignInManager<AvgIdentityUser> signInManager,
            IUserRoleManager<AvgIdentityUser, AvgDbContext> userService,
            IStringLocalizer<Labels> localizedLabels,
            IStringLocalizer<ErrorMessages> localizedErrorMessages)
            : base(userService, localizedLabels, localizedErrorMessages)
        {
            this.signInManager = signInManager;
        }

        public async Task<IActionResult> Exit()
        {
            await this.signInManager.SignOutAsync();
            return this.RedirectToAction("Index", "Home", new { area = string.Empty });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            var redirectUrl = this.Url.Action("ExternalLoginCallback", new { ReturnUrl = returnUrl });
            var properties = this.signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return this.Challenge(properties, provider);
        }

        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl, string remoteError = null)
        {
            if (remoteError != null)
            {
                this.ModelState.AddModelError(string.Empty, $"Error from external provider: {remoteError}");
                return this.View(nameof(this.Index));
            }

            var info = await this.signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return this.RedirectToAction(nameof(this.Index));
            }

            var result = await this.signInManager.ExternalLoginSignInAsync(
                             info.LoginProvider,
                             info.ProviderKey,
                             isPersistent: false);

            if (result.Succeeded)
            {
                return this.RedirectToLocal(returnUrl);
            }
            else
            {
                var email = info.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email);
                var firstName = info.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.GivenName);
                var lastName = info.Principal.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Surname);
                this.ViewBag.ReturnUrl = returnUrl;

                // this.ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return this.View(
                    "ExternalLoginConfirmation",
                    new AccountExternalLoginConfirmationViewModel
                        {
                            Email = email == null ? string.Empty : email.Value,
                            FirstName =
                                firstName == null
                                    ? string.Empty
                                    : firstName.Value,
                            LastName =
                                lastName == null
                                    ? string.Empty
                                    : lastName.Value
                        });
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(
            AccountExternalLoginConfirmationViewModel model,
            string returnUrl = null)
        {
            {
                // if (this.ModelState.IsValid)
                var info = await this.signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return this.View("ExternalLoginFailure");
                }

                //// TODO: avatar
                var user = await this.UserRoleManager.AddUserAsync(
                               model.Email,
                               model.FirstName,
                               model.LastName,
                               null,
                               null);

                if (user != null)
                {
                    //// TODO: Add to role
                    await this.UserRoleManager.AddUserExternalLoginInfoAsync(user, info);
                    await this.signInManager.SignInAsync(user, false);
                    return this.RedirectToLocal(returnUrl);
                }
                
                this.ModelState.AddModelError("Email", this.LocalizedErrorMessages["UsernameExist"]);
            }

            this.ViewData["ReturnUrl"] = returnUrl;
            return this.View(model);
        }

        [AllowAnonymous]
        public IActionResult ExternalLoginFailure()
        {
            return this.View();
        }

        [AllowAnonymous]
        public IActionResult Index(string returnUrl)
        {
            this.ViewData["ReturnUrl"] = returnUrl;
            return this.View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(AccountLoginViewModel model, string returnUrl = null)
        {
            this.ViewData["ReturnUrl"] = returnUrl;

            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            var result = await this.signInManager.PasswordSignInAsync(
                             model.Email,
                             model.Password,
                             true,
                             lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return this.RedirectToLocal(returnUrl);
            }

            this.ModelState.AddModelError(string.Empty, this.LocalizedErrorMessages["InvalidCredentials"]);
            return this.View(model);
        }
    }
}