using Common.Repositories;

namespace AdmissioningService.Core.Domain
{
    public class FacultyCache : BaseEntity
    {
        public required string Name { get; set; }
        public required bool Deprecated { get; set; }
    }
}
