using System.ComponentModel.DataAnnotations;

namespace ApplicantService.Presentation.Web.Validations
{
    public class FileAllowTypesValidationAttribute : ValidationAttribute
    {
    
        public FileAllowTypesValidationAttribute()
        {
            ErrorMessage = $"File type is not allowed. These extensions are allowed only: {MimeTypeMap.GetExistTypeString()}.";
        }

        public override bool IsValid(object? value)
        {
            if (value is IFormFile formFile)
            {
                string fileType = Path.GetExtension(formFile.FileName);
                if (!MimeTypeMap.ExistType(fileType))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
