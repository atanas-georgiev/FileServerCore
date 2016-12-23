namespace FileServerCore.Web.Areas.Account.Models
{
    using FileServerCore.Web.Resources;
    using System.ComponentModel.DataAnnotations;

    public class AccountExternalLoginConfirmationViewModel
    {
        [Display(Name = "Email", ResourceType = typeof(Labels))]
        [Required(ErrorMessageResourceName = "ErrorRequired", ErrorMessageResourceType = typeof(ErrorMessages))]
        [EmailAddress(ErrorMessageResourceName = "InvalidEmail", ErrorMessageResourceType = typeof(ErrorMessages))]        
        public string Email { get; set; }

        [Display(Name = "FirstName", ResourceType = typeof(Labels))]
        [Required(ErrorMessageResourceName = "ErrorRequired", ErrorMessageResourceType = typeof(ErrorMessages))]        
        [StringLength(50, ErrorMessageResourceName = "ErrorLength", ErrorMessageResourceType = typeof(ErrorMessages), MinimumLength = 2)]        
        public string FirstName { get; set; }

        [Display(Name = "LastName", ResourceType = typeof(Labels))]
        [Required(ErrorMessageResourceName = "ErrorRequired", ErrorMessageResourceType = typeof(ErrorMessages))]
        [StringLength(50, ErrorMessageResourceName = "ErrorLength", ErrorMessageResourceType = typeof(ErrorMessages), MinimumLength = 2)]        
        public string LastName { get; set; }
    }
}