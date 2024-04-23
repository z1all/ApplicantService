using Common.Repositories;

namespace AdmissioningService.Core.Domain
{
    public class Manager : BaseEntity
    {
        public IEnumerable<ApplicantAdmission> ApplicantAdmissions { get; set; } = null!;

        public Guid? FacultyId { get; set; }
        public FacultyCache? Faculty { get; set; }

        public UserCache? User { get; set; }
    }
}
