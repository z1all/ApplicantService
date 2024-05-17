using System.ComponentModel.DataAnnotations;

namespace Common.API.Attributes
{
    public class DataOnlyLessThenNowAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                DateOnly dateValue = (DateOnly)value;
                if (dateValue >= DateOnly.FromDateTime(DateTime.Now))
                {
                    return new ValidationResult(ErrorMessage);
                }
            }
            return ValidationResult.Success;
        }
    }
}
