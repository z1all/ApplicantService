using Common.Repositories;

namespace AdmissioningService.Core.Domain
{
    public class AdmissionCompany : BaseEntity
    {
        public required int EventYear { get; set; }
        public required bool IsCurrent { get; set; }

        public IEnumerable<ApplicantAdmission> ApplicantAdmissions { get; set; } = null!;
    }
}
