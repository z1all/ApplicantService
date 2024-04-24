using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Application.Interfaces.StateMachines;
using AdmissioningService.Core.Domain;

namespace AdmissioningService.Infrastructure.Persistence.StateMachines
{
    [Obsolete]
    public class ApplicantAdmissionStateMachin : IApplicantAdmissionStateMachin
    {
        private readonly IApplicantAdmissionRepository _applicantAdmissionRepository;

        public ApplicantAdmissionStateMachin(IApplicantAdmissionRepository applicantAdmissionRepository)
        {
            _applicantAdmissionRepository = applicantAdmissionRepository;
        }

        public async Task AddAsync(Guid applicantId, AdmissionCompany admissionCompany)
        {
            ApplicantAdmission applicantAdmission = new()
            {
                LastUpdate = DateTime.UtcNow,
                AdmissionStatus = Common.Models.Enums.AdmissionStatus.Created,
                ApplicantId = applicantId,
                AdmissionCompanyId = admissionCompany.Id
            };

            await _applicantAdmissionRepository.AddAsync(applicantAdmission);
        }

        public async Task<ApplicantAdmission?> GetByAdmissionCompanyId(Guid admissionCompanyId)
        {
            return await _applicantAdmissionRepository.GetByAdmissionCompanyId(admissionCompanyId);
        }
    }
}
