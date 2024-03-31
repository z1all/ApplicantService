using System.ComponentModel.DataAnnotations;

namespace ApplicantService.Core.Domain
{
    public class EducationDocumentTypeCache
    {
        [Key]
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required bool Deprecated { get; set; }
    }
}
