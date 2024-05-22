using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Common.API.Attributes
{
    public class FileMaxSizeValidationAttribute : ValidationAttribute
    {
        private readonly int _maxFileSize;

        public FileMaxSizeValidationAttribute(int maxFileSize)
        {
            _maxFileSize = maxFileSize;
            ErrorMessage = $"Maximum allowed file size is {_maxFileSize / (1024.0 * 1024.0)} megabytes.";
        }

        public override bool IsValid(object? value)
        {
            if (value is IFormFile formFile)
            {
                if (formFile.Length > _maxFileSize)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
