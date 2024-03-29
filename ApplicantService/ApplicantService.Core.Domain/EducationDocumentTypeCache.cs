namespace ApplicantService.Core.Domain
{
    public class EducationDocumentTypeCache
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required bool Deprecated { get; set; }
    }
}
