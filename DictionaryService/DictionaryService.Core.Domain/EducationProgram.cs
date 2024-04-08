using Common.Repositories;

namespace DictionaryService.Core.Domain
{
    public class EducationProgram : BaseEntity
    {
        public required DateTime CreatedTime { get; set; }
        public required string Name { get; set; }
        public required string Code { get; set; }
        public required string Language { get; set; }
        public required string EducationForm { get; set; }
        public required bool Deprecated { get; set; }

        public required Guid EducationLevelId { get; set; }
        public EducationLevel? EducationLevel { get; set; }

        public required Guid FacultyId { get; set; }
        public Faculty? Faculty { get; set; }
    }
}
