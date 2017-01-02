namespace FileServerCore.Web.Areas.Account.Models
{
    using System.ComponentModel.DataAnnotations;

    using FileServerCore.Web.Resources;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class AccountRegisterViewModel
    {
        public IFormFile Avatar { get; set; }

        [Display(Name = "ConfirmPassword", ResourceType = typeof(Labels))]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessageResourceName = "ErrorPasswordNotMatch",
            ErrorMessageResourceType = typeof(ErrorMessages))]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Email", ResourceType = typeof(Labels))]
        [Required(ErrorMessageResourceName = "ErrorRequired", ErrorMessageResourceType = typeof(ErrorMessages))]
        [EmailAddress(ErrorMessageResourceName = "InvalidEmail", ErrorMessageResourceType = typeof(ErrorMessages))]
        public string Email { get; set; }

        [Display(Name = "FirstName", ResourceType = typeof(Labels))]
        [Required(ErrorMessageResourceName = "ErrorRequired", ErrorMessageResourceType = typeof(ErrorMessages))]
        [StringLength(50, ErrorMessageResourceName = "ErrorLength", ErrorMessageResourceType = typeof(ErrorMessages),
            MinimumLength = 2)]
        public string FirstName { get; set; }

        [Display(Name = "LastName", ResourceType = typeof(Labels))]
        [Required(ErrorMessageResourceName = "ErrorRequired", ErrorMessageResourceType = typeof(ErrorMessages))]
        [StringLength(50, ErrorMessageResourceName = "ErrorLength", ErrorMessageResourceType = typeof(ErrorMessages),
            MinimumLength = 2)]
        public string LastName { get; set; }

        [Display(Name = "Password", ResourceType = typeof(Labels))]
        [Required(ErrorMessageResourceName = "ErrorRequired", ErrorMessageResourceType = typeof(ErrorMessages))]
        [StringLength(50, ErrorMessageResourceName = "ErrorMinLength", ErrorMessageResourceType = typeof(ErrorMessages),
            MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string ReturnUrl { get; set; }
    }
}