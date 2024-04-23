using Common.Repositories;

namespace AdmissioningService.Core.Domain
{
    public class EducationDocumentTypeCache : BaseEntity
    {
        public required string Name { get; set; }
        public required bool Deprecated { get; set; }

        public required Guid EducationLevelId { get; set; }
        public required EducationLevelCache EducationLevel { get; set; }

        public IEnumerable<EducationLevelCache> NextEducationLevel { get; set; } = null!;
    }
}
