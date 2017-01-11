namespace FileServerCore.Web.Areas.Shared.Controllers
{
    using System.Linq;

    using Avg.Data.Models;

    using FileServerCore.Web.Resources;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Localization;
    using AvgIdentity.Managers;
    using Avg.Data;

    public class BaseController : Controller
    {
        protected readonly IStringLocalizer<ErrorMessages> LocalizedErrorMessages;

        protected readonly IStringLocalizer<Labels> LocalizedLabels;

        public BaseController(
            IUserRoleManager<AvgIdentityUser, AvgDbContext> userRoleManager,
            IStringLocalizer<Labels> localizedLabels,
            IStringLocalizer<ErrorMessages> localizedErrorMessages)
        {
            this.UserRoleManager = userRoleManager;
            this.LocalizedLabels = localizedLabels;
            this.LocalizedErrorMessages = localizedErrorMessages;
        }

        protected AvgIdentityUser UserProfile { get; private set; }

        protected IUserRoleManager<AvgIdentityUser, AvgDbContext> UserRoleManager { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            this.UserProfile =
                this.UserRoleManager.GetAllUsers().FirstOrDefault(u => u.UserName == context.HttpContext.User.Identity.Name);
            base.OnActionExecuting(context);
        }

        protected IActionResult RedirectToLocal(string returnUrl)
        {
            if (this.Url.IsLocalUrl(returnUrl))
            {
                return this.Redirect(returnUrl);
            }

            return this.RedirectToAction("Index", "Home", new { area = string.Empty });
        }
    }
}