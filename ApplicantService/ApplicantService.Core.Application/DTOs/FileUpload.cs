using ApplicantService.Core.Application.Validations;
using Microsoft.AspNetCore.Http;

namespace ApplicantService.Core.Application.DTOs
{
    public class FileUpload
    {
        [FileMaxSizeValidation(10 * 1024 * 1024)]
        [FileAllowTypesValidation]
        public required IFormFile File { get; set; }
    }
}
