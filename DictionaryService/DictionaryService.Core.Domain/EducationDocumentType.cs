namespace DictionaryService.Core.Domain
{
    public class EducationDocumentType : BaseDictionaryEntity
    {
        public required Guid EducationLevelId { get; set; }
        public EducationLevel? EducationLevel { get; set; }

        public IEnumerable<EducationLevel> NextEducationLevels { get; set; } = null!;
    }
}
