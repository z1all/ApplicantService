using Common.Repositories;

namespace ApplicantService.Core.Domain
{
    public class EducationDocumentTypeCache : BaseEntity
    {
        public required string Name { get; set; }
        public required bool Deprecated { get; set; }
    }
}
