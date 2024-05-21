using System.ComponentModel.DataAnnotations;

namespace Common.Models.Attributes
{
    public class DateValidationAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            bool isCorrect = true;
            if (value is DateOnly date)
            {
                if (date >= DateOnly.FromDateTime(DateTime.UtcNow))
                {
                    //ErrorMessage = $"You cannot specify a date earlier than {DateOnly.FromDateTime(DateTime.UtcNow)}";
                    isCorrect = false;
                }
            }
            return isCorrect;
        }
    }
}
