namespace FileServerCore.Web.Areas.Account.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.AspNetCore.Mvc;

    using FileServerCore.Data;
    using FileServerCore.Data.Models;
    using FileServerCore.Services.Users;
    using FileServerCore.Web.Areas.Account.Models;
    using FileServerCore.Web.Areas.Shared.Controllers;
    using FileServerCore.Web.Resources;
    using Microsoft.Extensions.Localization;

    [Area("Account")]
    public class RegisterController : BaseController
    {
        public RegisterController(IUserService userService, IStringLocalizer<Labels> localizedLabels, IStringLocalizer<ErrorMessages> localizedErrorMessages, UserManager<User> userManager, SignInManager<User> signInManager, FileServerCoreDbContext dbContext) : base(userService, localizedLabels, localizedErrorMessages, userManager, signInManager, dbContext)
        {
        }

        public ActionResult Index(string returnUrl)
        {
            return this.View(new AccountRegisterViewModel() { returnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<ActionResult> Index(AccountRegisterViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    CreatedOn = DateTime.UtcNow
                };

                var result = await this.userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var role = this.dbContext.Roles.First(x => x.Name == "User"/*MyServerRoles.User.ToString()*/);
                    this.dbContext.UserRoles.Add(new IdentityUserRole<string>() { RoleId = role.Id, UserId = user.Id });
                    this.dbContext.SaveChanges();

                    await this.signInManager.SignInAsync(user, isPersistent: false);
                    if (string.IsNullOrEmpty(model.returnUrl))
                    {
                        return this.RedirectToAction("Index", "Home", new { area = string.Empty });
                    }

                    return RedirectToLocal(model.returnUrl);
                }
                
                this.ModelState.AddModelError("Email", LocalizedErrorMessages["UsernameExist"]);
            }

            return this.View(model);
        }
    }
}