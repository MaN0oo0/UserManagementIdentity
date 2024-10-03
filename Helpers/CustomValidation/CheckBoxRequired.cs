using System.ComponentModel.DataAnnotations;
using UserManagmentWithIdentity.ViewModels;

namespace UserManagmentWithIdentity.Helpers.CustomValidation
{
    public class CheckBoxRequired : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //get the entered value
            var Role = (AddUserViewModel)validationContext.ObjectInstance;
            //Check whether the IsAccepted is selected or not.
            if (Role.Roles.Any(x=>x.IsSelected) == false)
            {
                //if not checked the checkbox, return the error message.
                return new ValidationResult(ErrorMessage == null ? "Please checked the checkbox" : ErrorMessage);
            }
            return ValidationResult.Success;
        }
    }
}
