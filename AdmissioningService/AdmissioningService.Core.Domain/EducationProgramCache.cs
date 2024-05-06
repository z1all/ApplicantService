using Common.Repositories;

namespace AdmissioningService.Core.Domain
{
    public class EducationProgramCache : BaseEntity
    {
        public required string Name { get; set; }
        public required string Code { get; set; }
        public required string Language { get; set; }
        public required string EducationForm { get; set; }
        public required bool Deprecated { get; set; }

        public required Guid FacultyId { get; set; }
        public FacultyCache? Faculty { get; set; }

        public required Guid EducationLevelId { get; set; }
        public EducationLevelCache? EducationLevel { get; set; }
    }
}
