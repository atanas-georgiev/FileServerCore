namespace FileServerCore.Web.Areas.Admin.Controllers
{
    using System.Linq;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    
    using FileServerCore.Web.Areas.Admin.Models;
    using FileServerCore.Web.Areas.Shared.Controllers;    
    using FileServerCore.Web.Resources;

    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;
    using System.Threading.Tasks;
    using AvgIdentity.Managers;
    using Avg.Data.Models;
    using Avg.Data;

    // [Authorize(Roles = "Admin")]
    [Area("Admin")]
    public class JsonController : BaseController
    {
        public JsonController(
            IUserRoleManager<AvgIdentityUser, AvgDbContext> userService,
            IStringLocalizer<Labels> localizedLabels,
            IStringLocalizer<ErrorMessages> localizedErrorMessages)
            : base(userService, localizedLabels, localizedErrorMessages)
        {
        }

        [HttpPost]
        public ActionResult RolesCreate([DataSourceRequest] DataSourceRequest request, RolesViewModel role)
        {
            if (role != null && this.ModelState.IsValid)
            {
                this.UserRoleManager.AddRoles(new[] { role.Name });
            }

            return this.Json(new[] { role }.ToDataSourceResult(request, this.ModelState));
        }

        [HttpPost]
        public ActionResult RolesDestroy([DataSourceRequest] DataSourceRequest request, RolesViewModel role)
        {
            if (role != null)
            {
                var result = this.UserRoleManager.RemoveRoles(new[] { role.Name });

                if (result == false)
                {
                    this.ModelState.AddModelError("Name", "dadda");
                }
            }

            return this.Json(new[] { role }.ToDataSourceResult(request, this.ModelState));
        }

        public ActionResult RolesRead([DataSourceRequest] DataSourceRequest request)
        {            
            return this.Json(this.UserRoleManager.GetAllRoles().ProjectTo<RolesViewModel>().ToDataSourceResult(request));
        }
    }
}