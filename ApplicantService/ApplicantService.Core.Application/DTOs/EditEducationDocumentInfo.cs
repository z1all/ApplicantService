using System.ComponentModel.DataAnnotations;

namespace ApplicantService.Core.Application.DTOs
{
    public class EditEducationDocumentInfo
    {
        [MinLength(5)]
        public required string Name { get; set; }
    }
}
