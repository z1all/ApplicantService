using System.ComponentModel.DataAnnotations;

namespace AmdinPanelMVC.DTOs
{
    public class ChangeEducationDocumentDTO
    {
        public required Guid ApplicantId { get; set; }
        public required Guid DocumentId { get; set; }
        [MinLength(5, ErrorMessage = "Минимальная длина составляет 5 символов")]
        public required string Name { get; set; }
        public required Guid EducationDocumentTypeId { get; set; }
    }
}
