namespace FileServerCore.Web.Areas.Admin.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using AutoMapper;

    using Avg.Services.Users;

    using FileServerCore.Web.Infrastructure.Middlewares;

    public class RolesViewModel : Profile
    {
        public RolesViewModel(IUserService userService)
        {
            this.CreateMap<string, RolesViewModel>().ForMember(m => m.Name, opt => opt.MapFrom(c => c));
        }

        [UIHint("KendoTextBox")]
        public string Name { get; set; }

        [ScaffoldColumn(false)]
        public int UsersCount { get; set; }
    }

    //public class RolesViewModelMapping : Profile
    //{
    //    public RolesViewModelMapping()
    //    {
    //        this.CreateMap<string, RolesViewModel>().ForMember(m => m.Name, opt => opt.MapFrom(c => c));
    //    }
    //}
}