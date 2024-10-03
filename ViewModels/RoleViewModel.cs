using System.ComponentModel.DataAnnotations;
using UserManagmentWithIdentity.Helpers.CustomValidation;

namespace UserManagmentWithIdentity.ViewModels
{
    public class RoleViewModel
    {
        [Required]
        public string RoleId { get; set; }
        [Required]
        public string RoleName { get; set; }
        //[CheckBoxRequiredAttribute(ErrorMessage = "Please checked the items")]
        [Required(ErrorMessage = "You must select a role.")]
        public bool IsSelected { get; set; }

    }
}
