namespace FileServerCore.Web.Areas.Admin.Models
{
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;

    using Microsoft.AspNetCore.Mvc;
    using Avg.Data.Models;

    public class UsersViewModel : Profile
    {
        public UsersViewModel()
        {
            this.CreateMap<AvgIdentityUser, UsersViewModel>();
            //    .ForMember(m => m.Role, opt => opt.MapFrom(src => MappingFunctions.MapUserRole(src)));
        }

        [UIHint("KendoTextBox")]
        public string Email { get; set; }

        [UIHint("KendoTextBox")]
        public string FirstName { get; set; }

        [Key]
        [HiddenInput(DisplayValue = false)]
        public string Id { get; set; }

        [UIHint("KendoTextBox")]
        public string LastName { get; set; }

        //[UIHint("KendoDropDownRoles")]
        //[Display(Name = "Role", ResourceType = typeof(Helpers_SharedResource))]
        //public MyServerRoles Role { get; set; }
    }
}