namespace FileServerCore.Web.Areas.Shared.Controllers
{
    using System.Linq;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    using FileServerCore.Data;
    using FileServerCore.Data.Models;
    using FileServerCore.Services.Users;
    using Microsoft.Extensions.Localization;
    using FileServerCore.Web.Resources;

    public class BaseController : Controller
    {
        protected readonly FileServerCoreDbContext dbContext;

        protected readonly SignInManager<User> signInManager;

        protected readonly UserManager<User> userManager;

        protected readonly IStringLocalizer<Labels> LocalizedLabels;

        protected readonly IStringLocalizer<ErrorMessages> LocalizedErrorMessages;

        public BaseController(
            IUserService userService,
            IStringLocalizer<Labels> localizedLabels,
            IStringLocalizer<ErrorMessages> localizedErrorMessages,
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            FileServerCoreDbContext dbContext)
        {
            this.UserService = userService;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.dbContext = dbContext;
            this.LocalizedLabels = localizedLabels;
            this.LocalizedErrorMessages = localizedErrorMessages;
        }

        protected User UserProfile { get; private set; }

        protected IUserService UserService { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            this.UserProfile =
                this.UserService.GetAll().FirstOrDefault(u => u.UserName == context.HttpContext.User.Identity.Name);
            base.OnActionExecuting(context);
        }

        protected void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                // this.ModelState.AddModelError(string.Empty, error);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.userManager != null)
                {
                    this.userManager.Dispose();
                }
            }

            base.Dispose(disposing);
        }

        protected ActionResult RedirectToLocal(string returnUrl)
        {
            if (this.Url.IsLocalUrl(returnUrl))
            {
                return this.Redirect(returnUrl);
            }

            return this.RedirectToAction("Index", "Home", new { area = string.Empty });
        }
    }
}