using System.ComponentModel.DataAnnotations;

namespace ApplicantService.Core.Application.DTOs
{
    public class AddEducationDocumentInfo
    {
        [MinLength(5)]
        public required string Name { get; set; }
        [Required]
        public required Guid EducationDocumentTypeId { get; set; }
    }
}
