using Common.Repositories;

namespace AdmissioningService.Core.Domain
{
    public class EducationLevelCache : BaseEntity
    {
        public required string Name { get; set; }
        public required bool Deprecated { get; set; }
    }
}
