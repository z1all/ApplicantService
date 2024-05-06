using Common.Models.Enums;
using Common.Repositories;

namespace AdmissioningService.Core.Domain
{
    public class ApplicantAdmission : BaseEntity
    {
        public required DateTime LastUpdate { get; set; }
        public required AdmissionStatus AdmissionStatus { get; set; }

        public required Guid AdmissionCompanyId { get; set; }
        public AdmissionCompany? AdmissionCompany { get; set; }

        public required Guid ApplicantId { get; set; }
        public ApplicantCache? Applicant { get; set; }

        public Guid? ManagerId { get; set; }
        public Manager? Manager { get; set; }

        public IEnumerable<AdmissionProgram> AdmissionPrograms { get; set; } = null!;
    }
}
