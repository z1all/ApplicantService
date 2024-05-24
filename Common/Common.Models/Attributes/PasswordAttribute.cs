using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Common.Models.Attributes
{
    public class PasswordAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is string password)
            {
                string pattern = @"^(?=.*[A-Z])(?=.*\d).{6,}$";
                if (!Regex.IsMatch(password, pattern))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
