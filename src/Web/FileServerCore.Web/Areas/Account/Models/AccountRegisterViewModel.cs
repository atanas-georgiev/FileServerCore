namespace FileServerCore.Web.Areas.Account.Models
{
    using System.ComponentModel.DataAnnotations;    
    using Microsoft.AspNetCore.Mvc;
    using FileServerCore.Web.Resources;

    public class AccountRegisterViewModel
    {
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Resources_Labels))]
        [DataType(DataType.Password)]        
        [Compare("Password", ErrorMessageResourceName = "ErrorPasswordNotMatch", ErrorMessageResourceType = typeof(Resources_ErrorMessages))]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Email", ResourceType = typeof(Resources_Labels))]
        [Required(ErrorMessageResourceName = "ErrorRequired", ErrorMessageResourceType = typeof(Resources_ErrorMessages))]
        [EmailAddress(ErrorMessageResourceName = "InvalidEmail", ErrorMessageResourceType = typeof(Resources_ErrorMessages))]        
        public string Email { get; set; }

        [Display(Name = "FirstName", ResourceType = typeof(Resources_Labels))]
        [Required(ErrorMessageResourceName = "ErrorRequired", ErrorMessageResourceType = typeof(Resources_ErrorMessages))]
        [StringLength(50, ErrorMessageResourceName = "ErrorLength", ErrorMessageResourceType = typeof(Resources_ErrorMessages), MinimumLength = 2)]        
        public string FirstName { get; set; }

        [Display(Name = "LastName", ResourceType = typeof(Resources_Labels))]
        [Required(ErrorMessageResourceName = "ErrorRequired", ErrorMessageResourceType = typeof(Resources_ErrorMessages))]
        [StringLength(50, ErrorMessageResourceName = "ErrorLength", ErrorMessageResourceType = typeof(Resources_ErrorMessages), MinimumLength = 2)]        
        public string LastName { get; set; }

        [Display(Name = "Password", ResourceType = typeof(Resources_Labels))]
        [Required(ErrorMessageResourceName = "ErrorRequired", ErrorMessageResourceType = typeof(Resources_ErrorMessages))]
        [StringLength(50, ErrorMessageResourceName = "ErrorMinLength", ErrorMessageResourceType = typeof(Resources_ErrorMessages), MinimumLength = 6)]
        [DataType(DataType.Password)]        
        public string Password { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string returnUrl { get; set; }
    }
}