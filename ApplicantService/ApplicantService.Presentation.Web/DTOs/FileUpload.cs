using ApplicantService.Presentation.Web.Validations;

namespace ApplicantService.Presentation.Web.DTOs
{
    public class FileUpload
    {
        [FileMaxSizeValidation(10 * 1024 * 1024)]
        [FileAllowTypesValidation]
        public required IFormFile File { get; set; }
    }
}
