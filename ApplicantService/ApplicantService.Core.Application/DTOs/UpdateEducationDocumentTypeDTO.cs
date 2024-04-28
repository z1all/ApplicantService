namespace ApplicantService.Core.Application.DTOs
{
    public class UpdateEducationDocumentTypeDTO
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required bool Deprecated { get; set; }
    }
}
