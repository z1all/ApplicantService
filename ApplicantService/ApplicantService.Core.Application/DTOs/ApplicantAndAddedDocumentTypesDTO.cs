using System.ComponentModel.DataAnnotations;

namespace ApplicantService.Core.Application.DTOs
{
    public class ApplicantAndAddedDocumentTypesDTO
    {
        [Required]
        public required Guid Id { get; set; }
        [MinLength(5)]
        public required string FullName { get; set; }
        [EmailAddress]
        [Required]
        public required string Email { get; set; }
        public required List<Guid> AddedDocumentTypesId { get; set; }
    }
}
