namespace ApplicantService.Core.Domain
{
    public class EducationDocument : Document
    {
        public required string Name { get; set; }

        public required Guid EducationDocumentTypeId { get; set; }
        public EducationDocumentTypeCache? EducationDocumentType { get; set; }

        public required Guid ApplicantIdCache { get; set; }
    }
}
