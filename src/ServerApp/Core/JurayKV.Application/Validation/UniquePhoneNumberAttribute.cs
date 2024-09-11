using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace JurayKV.Application.Validation
{
    public class ValidatePhoneNumberAttribute : ValidationAttribute
    {
        //protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        //{
        //    var userManager = (UserManager<IdentityUser>)validationContext.GetService(typeof(UserManager<IdentityUser>));
        //    var user = userManager.FindByNameAsync(value.ToString()).Result;

        //    if (user != null)
        //    {
        //        return new ValidationResult("Phone number is already in use.");
        //    }

        //    return ValidationResult.Success;
        //}

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Remove spaces and hyphens from the input phone number
            value = Regex.Replace(value.ToString(), @"\s|-", "");

            // Check if the phone number matches the Nigerian format
            if (Regex.IsMatch(value.ToString(), @"^(?:\+234|0)[789]\d{9}$"))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Invalid Phone Number");
        }
    }
}
