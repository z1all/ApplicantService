using Common.Repositories;

namespace DictionaryService.Core.Domain
{
    public class EducationDocumentType : BaseEntity
    {
        public required string Name { get; set; }
        public required bool Deprecated { get; set; }

        public required Guid EducationLevelId { get; set; }
        public EducationLevel? EducationLevel { get; set; }

        public IEnumerable<EducationLevel> NextEducationLevels { get; set; } = null!;
    }
}
