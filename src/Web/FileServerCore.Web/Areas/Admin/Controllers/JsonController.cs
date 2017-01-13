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
                var res = this.UserRoleManager.AddRoleAsync(role.Name).Result;
            }

            return this.Json(new[] { role }.ToDataSourceResult(request, this.ModelState));
        }

        [HttpPost]
        public ActionResult RolesDestroy([DataSourceRequest] DataSourceRequest request, RolesViewModel role)
        {
            if (role != null)
            {
                var result = this.UserRoleManager.RemoveRoleAsync(role.Name);

                if (result.Result == false)
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

        [HttpPost]
        public ActionResult UsersCreate([DataSourceRequest] DataSourceRequest request, UsersViewModel user)
        {
            if (user != null && this.ModelState.IsValid)
            {
                //var dbuser = new User() { FirstName = user.FirstName, LastName = user.LastName, Email = user.Email };
                var user2 = this.UserRoleManager.AddUserAsync(user.Email, null, null, null, user.FirstName, user.LastName, null).Result;
            }

            return this.Json(new[] { user }.ToDataSourceResult(request, this.ModelState));
        }

        [HttpPost]
        public ActionResult UsersDestroy([DataSourceRequest] DataSourceRequest request, UsersViewModel user)
        {
            if (user != null)
            {
                //this.UserService.Delete(user.Id);
            }

            return this.Json(new[] { user }.ToDataSourceResult(request, this.ModelState));
        }

        public ActionResult UsersRead([DataSourceRequest] DataSourceRequest request)
        {
            var a = this.UserRoleManager.GetAllUsers().ProjectTo<UsersViewModel>();
            return this.Json(this.UserRoleManager.GetAllUsers().ProjectTo<UsersViewModel>().ToDataSourceResult(request));
        }

        [HttpPost]
        public ActionResult UsersUpdate([DataSourceRequest] DataSourceRequest request, UsersViewModel user)
        {
            if (user != null && this.ModelState.IsValid)
            {
                //var dbuser = this.UserService.GetById(user.Id);
                //if (dbuser != null)
                //{
                //    dbuser.FirstName = user.FirstName;
                //    dbuser.LastName = user.LastName;
                //    dbuser.Email = user.Email;

                //    this.UserService.Update(dbuser, user.Role.ToString());
                //}
            }

            return this.Json(new[] { user }.ToDataSourceResult(request, this.ModelState));
        }
    }
}