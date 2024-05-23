using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Application.Interfaces.Services;
using AdmissioningService.Core.Application.Interfaces.StateMachines;
using AdmissioningService.Core.Domain;
using Common.Models.Enums;
using Common.Models.Models;

namespace AdmissioningService.Core.Application.StateMachines
{

    public class ApplicantAdmissionStateMachin : IApplicantAdmissionStateMachin
    {
        private readonly IApplicantAdmissionRepository _applicantAdmissionRepository;
        private readonly IAdmissionProgramRepository _admissionProgramRepository;
        private readonly IApplicantCacheRepository _applicantCacheRepository;
        private readonly INotificationService _notificationService;

        public ApplicantAdmissionStateMachin(
            IApplicantAdmissionRepository applicantAdmissionRepository, IAdmissionProgramRepository admissionProgramRepository,
            IApplicantCacheRepository applicantCacheRepository, INotificationService notificationService)
        {
            _applicantAdmissionRepository = applicantAdmissionRepository;
            _admissionProgramRepository = admissionProgramRepository;
            _applicantCacheRepository = applicantCacheRepository;
            _notificationService = notificationService;
        }

        [Obsolete]
        public async Task CreateApplicantAdmissionAsync(Guid applicantId, AdmissionCompany admissionCompany)
        {
            ApplicantAdmission applicantAdmission = new()
            {
                LastUpdate = DateTime.UtcNow,
                AdmissionStatus = AdmissionStatus.Created,
                ApplicantId = applicantId,
                AdmissionCompanyId = admissionCompany.Id
            };

            await _applicantAdmissionRepository.AddAsync(applicantAdmission);

            await SendNotificationIfNewAsync(applicantAdmission.ApplicantId, applicantAdmission.AdmissionStatus, null);
        }

        [Obsolete]
        public async Task AddManagerAsync(ApplicantAdmission applicantAdmission, Manager manager)
        {
            AdmissionStatus oldStatus = applicantAdmission.AdmissionStatus;

            applicantAdmission.Manager = manager;

            if (applicantAdmission.AdmissionStatus == AdmissionStatus.Created)
            {
                applicantAdmission.AdmissionStatus = AdmissionStatus.UnderConsideration;
            }

            await _applicantAdmissionRepository.UpdateAsync(applicantAdmission);

            await SendNotificationIfNewAsync(applicantAdmission.ApplicantId, applicantAdmission.AdmissionStatus, oldStatus);
        }

        [Obsolete]
        public async Task DeleteManagerAsync(ApplicantAdmission applicantAdmission)
        {
            AdmissionStatus oldStatus = applicantAdmission.AdmissionStatus;

            applicantAdmission.ManagerId = null;

            if (applicantAdmission.AdmissionStatus == AdmissionStatus.UnderConsideration)
            {
                applicantAdmission.AdmissionStatus = AdmissionStatus.Created;
            }

            await _applicantAdmissionRepository.UpdateAsync(applicantAdmission);

            await SendNotificationIfNewAsync(applicantAdmission.ApplicantId, applicantAdmission.AdmissionStatus, oldStatus);
        }

        [Obsolete]
        public async Task DeleteManagerRangeAsync(List<ApplicantAdmission> applicantAdmissions)
        {
            List<ApplicantAdmission> changedStatuses = new List<ApplicantAdmission>();

            foreach (var admission in applicantAdmissions)
            {
                AdmissionStatus oldStatus = admission.AdmissionStatus;

                admission.ManagerId = null;

                if (admission.AdmissionStatus == AdmissionStatus.UnderConsideration)
                {
                    admission.AdmissionStatus = AdmissionStatus.Created;
                }

                if (oldStatus != admission.AdmissionStatus)
                {
                    changedStatuses.Add(admission);
                }
            }

            await _applicantAdmissionRepository.UpdateRangeAsync(applicantAdmissions);

            foreach(var changedStatus in changedStatuses)
            {
                await SendNotificationIfNewAsync(changedStatus.ApplicantId, changedStatus.AdmissionStatus, null);
            }
        }

        [Obsolete]
        public async Task<bool> ApplicantInfoUpdatedAsync(Guid applicantId)
        {
            ApplicantAdmission? applicantAdmission = await _applicantAdmissionRepository.GetCurrentByApplicantIdAsync(applicantId);
            if (applicantAdmission is null) return false;

            applicantAdmission.LastUpdate = DateTime.UtcNow;

            (AdmissionStatus newStatus, AdmissionStatus oldStatus) = UpdateAdmissionStatusFromRejectedAndConfirmed(applicantAdmission);

            await _applicantAdmissionRepository.UpdateAsync(applicantAdmission);

            await SendNotificationIfNewAsync(applicantId, newStatus, oldStatus);

            return true;
        }

        [Obsolete]
        public async Task<bool> AddAdmissionProgramAsync(Guid applicantId, AdmissionProgram admissionProgram)
        {
            ExecutionResult<(AdmissionStatus newStatus, AdmissionStatus oldStatus)> result = await UpdateAdmissionStatusAsync(admissionProgram.ApplicantAdmissionId);
            if (!result.IsSuccess) return false;

            await _admissionProgramRepository.AddAsync(admissionProgram);

            await SendNotificationIfNewAsync(applicantId, result.Result.newStatus, result.Result.oldStatus);

            return true;
        }

        [Obsolete]
        public async Task<bool> DeleteAdmissionProgramAsync(Guid applicantId, AdmissionProgram admissionProgram)
        {
            ExecutionResult<(AdmissionStatus newStatus, AdmissionStatus oldStatus)> result = await UpdateAdmissionStatusAsync(admissionProgram.ApplicantAdmissionId);
            if (!result.IsSuccess) return false;

            await _admissionProgramRepository.DeleteAsync(admissionProgram);

            await SendNotificationIfNewAsync(applicantId, result.Result.newStatus, result.Result.oldStatus);

            return true;
        }

        [Obsolete]
        public async Task<bool> UpdateAdmissionProgramRangeAsync(Guid applicantId, List<AdmissionProgram> admissionPrograms, Guid admissionId)
        {
            if (admissionPrograms.Count > 0)
            {
                ExecutionResult<(AdmissionStatus newStatus, AdmissionStatus oldStatus)> result = await UpdateAdmissionStatusAsync(admissionId);
                if (!result.IsSuccess) return false;

                await _admissionProgramRepository.UpdateRangeAsync(admissionPrograms);

                await SendNotificationIfNewAsync(applicantId, result.Result.newStatus, result.Result.oldStatus);
            }

            return true;
        }

        [Obsolete]
        public async Task ChangeAdmissionStatusAsync(ApplicantAdmission applicantAdmission, ManagerChangeAdmissionStatus changeAdmissionStatus)
        {
            AdmissionStatus oldStatus = applicantAdmission.AdmissionStatus;

            applicantAdmission.LastUpdate = DateTime.UtcNow;
            applicantAdmission.AdmissionStatus = (AdmissionStatus)changeAdmissionStatus;

            await _applicantAdmissionRepository.UpdateAsync(applicantAdmission);

            await SendNotificationIfNewAsync(applicantAdmission.ApplicantId, (AdmissionStatus)changeAdmissionStatus, oldStatus);
        }

        [Obsolete]
        private async Task<ExecutionResult<(AdmissionStatus newStatus, AdmissionStatus oldStatus)>> UpdateAdmissionStatusAsync(Guid admissionId)
        {
            ApplicantAdmission? applicantAdmission = await _applicantAdmissionRepository.GetByIdAsync(admissionId);
            if (applicantAdmission is null)
            {
                return new(StatusCodeExecutionResult.NotFound, keyError: "AdmissionNotFound", error: $"Admission with id {admissionId} not found!");
            }

            (AdmissionStatus newStatus, AdmissionStatus oldStatus) = UpdateAdmissionStatusFromRejectedAndConfirmed(applicantAdmission);

            await _applicantAdmissionRepository.UpdateAsync(applicantAdmission);

            return new() { Result = (newStatus, oldStatus) };
        }

        [Obsolete]
        private (AdmissionStatus newStatus, AdmissionStatus oldStatus) UpdateAdmissionStatusFromRejectedAndConfirmed(ApplicantAdmission applicantAdmission)
        {
            applicantAdmission.LastUpdate = DateTime.UtcNow;

            AdmissionStatus oldStatus = applicantAdmission.AdmissionStatus;

            AdmissionStatus[] rejectedOrConfirmed = [AdmissionStatus.Rejected, AdmissionStatus.Confirmed];
            if (rejectedOrConfirmed.Contains(applicantAdmission.AdmissionStatus))
            {
                applicantAdmission.AdmissionStatus
                    = (applicantAdmission.ManagerId is null ? AdmissionStatus.Created : AdmissionStatus.UnderConsideration);
            }

            return (applicantAdmission.AdmissionStatus, oldStatus);
        }

        private async Task<ExecutionResult> SendNotificationIfNewAsync(Guid applicantId, AdmissionStatus newStatus, AdmissionStatus? oldStatus)
        {
            if (oldStatus == newStatus) return new(isSuccess: true);

            ApplicantCache? applicantCache = await _applicantCacheRepository.GetByIdAsync(applicantId);
            if (applicantCache is null)
            {
                return new(StatusCodeExecutionResult.NotFound, keyError: "ApplicantNotFound", error: $"Applicant with id {applicantId} not found!");
            }

            return await _notificationService.UpdatedAdmissionStatusAsync(newStatus, new()
            {
                Id = applicantId,
                FullName = applicantCache.FullName,
                Email = applicantCache.Email,
            });
        }
    }
}