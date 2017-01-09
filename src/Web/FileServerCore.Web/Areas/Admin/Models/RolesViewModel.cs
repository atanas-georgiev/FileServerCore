namespace FileServerCore.Web.Areas.Admin.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using AutoMapper;

    using Avg.Services.Users;

    using FileServerCore.Web.Infrastructure.Middlewares;
    using Microsoft.AspNetCore.Http;
    using FileServerCore.Web.Infrastructure.Helpers;
    using System.Linq;
    using FileServerCore.Web.Areas.Shared.Models;

    public class RolesViewModel
    {
        [UIHint("KendoTextBox")]
        public string Name { get; set; }

//        [ScaffoldColumn(false)]
        public int UsersCount { get; set; }
    }

    public class RolesViewModelMapping : Profile
    {       
        public RolesViewModelMapping()
        {                        
            this.CreateMap<string, RolesViewModel>()
                .ForMember(m => m.Name, opt => opt.MapFrom(c => c))
                .ForMember(m => m.UsersCount, opt => opt.MapFrom(c => MappingFunctions.GetUsersCountInRole(c)));
        }
    }
}