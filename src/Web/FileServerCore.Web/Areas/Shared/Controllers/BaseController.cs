namespace FileServerCore.Web.Areas.Shared.Controllers
{
    using System.Linq;

    using Avg.Data.Models;
    using Avg.Services.Users;

    using FileServerCore.Web.Resources;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.Extensions.Localization;

    public class BaseController : Controller
    {
        protected readonly IStringLocalizer<ErrorMessages> LocalizedErrorMessages;

        protected readonly IStringLocalizer<Labels> LocalizedLabels;

        public BaseController(
            IUserService userService,
            IStringLocalizer<Labels> localizedLabels,
            IStringLocalizer<ErrorMessages> localizedErrorMessages)
        {
            this.UserService = userService;
            this.LocalizedLabels = localizedLabels;
            this.LocalizedErrorMessages = localizedErrorMessages;
        }

        protected AvgUser UserProfile { get; private set; }

        protected IUserService UserService { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            this.UserProfile =
                this.UserService.GetAll().FirstOrDefault(u => u.UserName == context.HttpContext.User.Identity.Name);
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