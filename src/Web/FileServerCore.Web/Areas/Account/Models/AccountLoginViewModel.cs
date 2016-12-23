namespace FileServerCore.Web.Areas.Account.Models
{
    using FileServerCore.Web.Resources;
    using System.ComponentModel.DataAnnotations;

    public class AccountLoginViewModel
    {
        [Display(Name = "Email", ResourceType = typeof(Labels))]
        [Required(ErrorMessageResourceName = "ErrorRequired", ErrorMessageResourceType = typeof(ErrorMessages))]
        [EmailAddress(ErrorMessageResourceName = "InvalidEmail", ErrorMessageResourceType = typeof(ErrorMessages))]        
        public string Email { get; set; }

        [Display(Name = "Password", ResourceType = typeof(Labels))]
        [Required(ErrorMessageResourceName = "ErrorRequired", ErrorMessageResourceType = typeof(ErrorMessages))]
        [StringLength(50, ErrorMessageResourceName = "ErrorMinLength", ErrorMessageResourceType = typeof(ErrorMessages), MinimumLength = 6)]
        [DataType(DataType.Password)]        
        public string Password { get; set; }

        [Display(Name = "RememberMe", ResourceType = typeof(Labels))]
        public bool RememberMe { get; set; }
    }
}