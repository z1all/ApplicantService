using System.ComponentModel.DataAnnotations;

namespace ApplicantService.Core.Application.DTOs
{
    public class UpdateEducationDocumentTypeDTO
    {
        public required Guid Id { get; set; }
        [MinLength(5)]
        public required string Name { get; set; }
        [MinLength(5)]
        public required bool Deprecated { get; set; }
    }
}
