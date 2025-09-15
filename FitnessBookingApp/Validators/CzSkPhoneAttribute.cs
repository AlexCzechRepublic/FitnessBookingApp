using System.ComponentModel.DataAnnotations;
using FitnessBookingApp.Utils;

namespace FitnessBookingApp.Validators
{
    public class CzSkPhoneAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var s = value as string;

            if (!PhoneUtils.TryNormalizeFull(s, out var normalized, out var error))
            {
                return new ValidationResult(error);
            }

            var prop = validationContext.ObjectType.GetProperty(validationContext.MemberName!);
            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(validationContext.ObjectInstance, normalized);
            }

            return ValidationResult.Success;
        }
    }
}