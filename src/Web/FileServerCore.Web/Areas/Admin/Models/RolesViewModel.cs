namespace FileServerCore.Web.Areas.Admin.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using AutoMapper;

    using Avg.Services.Mappings;

    public class RolesViewModel : IMapFrom<string>, IHaveCustomMappings
    {
        [UIHint("KendoTextBox")]
        public string Name { get; set; }

        [ScaffoldColumn(false)]
        public int UsersCount { get; set; }

        public void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<string, RolesViewModel>().ForMember(m => m.Name, opt => opt.MapFrom(c => c));
        }
    }
}