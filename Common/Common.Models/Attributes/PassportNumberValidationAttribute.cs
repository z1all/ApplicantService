using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Common.Models.Attributes
{
    public class PassportNumberValidationAttribute : ValidationAttribute
    {
        public PassportNumberValidationAttribute() { }

        public override bool IsValid(object? value)
        {
            if (value is string passportNumber)
            {
                string pattern = @"\d{4} \d{6}";
                if (!Regex.IsMatch(passportNumber, pattern))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
