namespace ApplicantService.Core.Application.DTOs
{
    public class EditAddEducationDocumentInfo
    {
        public required string Name { get; set; }
        public required Guid EducationDocumentTypeId { get; set; }
    }
}
