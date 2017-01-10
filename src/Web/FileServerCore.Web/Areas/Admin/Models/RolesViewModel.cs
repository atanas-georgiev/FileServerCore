namespace FileServerCore.Web.Areas.Admin.Models
{
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using FileServerCore.Web.Areas.Shared.Models;

    public class RolesViewModel : Profile
    {
        public RolesViewModel()
        {
            this.CreateMap<string, RolesViewModel>()
                .ForMember(m => m.Name, opt => opt.MapFrom(c => c))
                .ForMember(m => m.UsersCount, opt => opt.MapFrom(c => MappingFunctions.GetUsersCountInRole(c)));
        }

        [UIHint("KendoTextBox")]
        public string Name { get; set; }
                
        public int UsersCount { get; set; }
    }
}