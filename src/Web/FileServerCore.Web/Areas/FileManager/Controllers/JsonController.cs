namespace FileServerCore.Web.Areas.FileManager.Controllers
{
    using System.Collections.Generic;
    using System.IO;

    using Avg.Services.Users;

    using FileServerCore.Web.Areas.FileManager.Models;
    using FileServerCore.Web.Areas.Shared.Controllers;
    using FileServerCore.Web.Resources;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Localization;

    // [Authorize(Roles = "Admin")]
    [Area("FileManager")]
    public class JsonController : BaseController
    {
        private static List<KendoTreeViewViewModel> list = new List<KendoTreeViewViewModel>();

        private readonly IHostingEnvironment env;

        public JsonController(
            IHostingEnvironment env,
            IUserService userService,
            IStringLocalizer<Labels> localizedLabels,
            IStringLocalizer<ErrorMessages> localizedErrorMessages)
            : base(userService, localizedLabels, localizedErrorMessages)
        {
            this.env = env;
        }

        public JsonResult Read(string path)
        {
            list.Clear();
            DirSearch(this.env.WebRootPath);

            return this.Json(list);
        }

        static void DirSearch(string sDir)
        {
            foreach (string d in Directory.GetDirectories(sDir))
            {
                var files = new List<KendoTreeViewViewModel>();
                foreach (string f in Directory.GetFiles(d))
                {
                    files.Add(new KendoTreeViewViewModel() { Id = f, Name = f, HasChildren = false });
                }

                list.Add(new KendoTreeViewViewModel() { Id = d, Name = d, HasChildren = true, Children = files });
                list.AddRange(files);

                DirSearch(d);
            }
        }
    }
}