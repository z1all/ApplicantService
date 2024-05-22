using Common.API.Attributes;

namespace ApplicantService.Presentation.Web.DTOs
{
    public class FileUpload
    {
        [FileMaxSizeValidation(10 * 1024 * 1024)]
        [FileAllowTypesValidation]
        public required IFormFile File { get; set; }
    }
}
