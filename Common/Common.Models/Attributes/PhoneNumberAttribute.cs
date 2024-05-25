using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Common.Models.Attributes
{
    public class PhoneNumberAttribute : ValidationAttribute
    {
        public override bool IsValid(object? value)
        {
            if (value is string phoneNumber)
            {
                string pattern = @"^(?:\+7|8)[0-9]{10}$";
                if (!Regex.IsMatch(phoneNumber, pattern))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
