using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using FileServerCore.Data;
using FileServerCore.Data.Models;
using FileServerCore.Services.Users;
using FileServerCore.Web.Areas.Shared.Controllers;
using System.IO;
using System.Linq;

using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using FileServerCore.Web.Areas.FileManager.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using FileServerCore.Web.Resources;
using Microsoft.Extensions.Localization;

namespace FileServerCore.Web.Areas.FileManager.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Area("FileManager")]
    public class JsonController : BaseController
    {
        private readonly IHostingEnvironment env;

        public JsonResult Read(string path)
        {
            list.Clear();
            DirSearch(this.env.WebRootPath);
            
            return Json(list);            
        }

        private static List<KendoTreeViewViewModel> list = new List<KendoTreeViewViewModel>();

        public JsonController(IHostingEnvironment env, IUserService userService, IStringLocalizer<Labels> localizedLabels, IStringLocalizer<ErrorMessages> localizedErrorMessages, UserManager<User> userManager, SignInManager<User> signInManager, FileServerCoreDbContext dbContext) : base(userService, localizedLabels, localizedErrorMessages, userManager, signInManager, dbContext)
        {
            this.env = env;
        }

        static void DirSearch(string sDir)
        {
            foreach (string d in Directory.GetDirectories(sDir))
            {
                var files = new List<KendoTreeViewViewModel>();
                foreach (string f in Directory.GetFiles(d))
                {                    
                    files.Add(new KendoTreeViewViewModel()
                    {
                        Id = f,
                        Name = f,
                        HasChildren = false
                    });
                }

                list.Add(new KendoTreeViewViewModel()
                {
                    Id = d,
                    Name = d,
                    HasChildren = true,
                    Children = files
                });
                list.AddRange(files);

                DirSearch(d);
            }
        }
    }
}
