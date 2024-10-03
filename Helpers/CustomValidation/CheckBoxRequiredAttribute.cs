using System.ComponentModel.DataAnnotations;
using UserManagmentWithIdentity.ViewModels;

namespace UserManagmentWithIdentity.Helpers.CustomValidation
{
    public class CheckBoxRequiredAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            var roles = (List<RoleViewModel>)value;
            return roles.Any(r => r.IsSelected);
        }
    }
}
