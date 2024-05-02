using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Application.Interfaces.StateMachines;
using AdmissioningService.Core.Domain;
using Common.Models.Enums;

namespace AdmissioningService.Infrastructure.Persistence.StateMachines
{
    [Obsolete]
    public class ApplicantAdmissionStateMachin : IApplicantAdmissionStateMachin
    {
        private readonly IApplicantAdmissionRepository _applicantAdmissionRepository;
        private readonly IAdmissionProgramRepository _admissionProgramRepository;

        public ApplicantAdmissionStateMachin(
            IApplicantAdmissionRepository applicantAdmissionRepository, IAdmissionProgramRepository admissionProgramRepository)
        {
            _applicantAdmissionRepository = applicantAdmissionRepository;
            _admissionProgramRepository = admissionProgramRepository;
        }

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
        }

        public async Task AddManagerAsync(ApplicantAdmission applicantAdmission, Manager manager)
        {
            applicantAdmission.Manager = manager;

            if (applicantAdmission.AdmissionStatus == AdmissionStatus.Created)
            {
                applicantAdmission.AdmissionStatus = AdmissionStatus.UnderConsideration;
            }

            await _applicantAdmissionRepository.UpdateAsync(applicantAdmission);
        }

        public async Task DeleteManagerAsync(ApplicantAdmission applicantAdmission)
        {
            applicantAdmission.ManagerId = null;

            if (applicantAdmission.AdmissionStatus == AdmissionStatus.UnderConsideration)
            {
                applicantAdmission.AdmissionStatus = AdmissionStatus.Created;
            }

            await _applicantAdmissionRepository.UpdateAsync(applicantAdmission);
        }

        public async Task<bool> ApplicantInfoUpdatedAsync(Guid applicantId)
        {
            ApplicantAdmission? applicantAdmission = await _applicantAdmissionRepository.GetCurrentByApplicantIdAsync(applicantId);
            if (applicantAdmission is null) return false;

            applicantAdmission.LastUpdate = DateTime.UtcNow;

            UpdateAdmissionStatusFromRejectedAndConfirmed(applicantAdmission);

            await _applicantAdmissionRepository.UpdateAsync(applicantAdmission);

            return true;
        }

        public async Task<bool> AddAdmissionProgramAsync(AdmissionProgram admissionProgram)
        {
            bool isSuccess = await UpdateAdmissionStatusAsync(admissionProgram.ApplicantAdmissionId);
            if (!isSuccess) return false;

            await _admissionProgramRepository.AddAsync(admissionProgram);

            return true;
        }

        public async Task<bool> DeleteAdmissionProgramAsync(AdmissionProgram admissionProgram)
        {
            bool isSuccess = await UpdateAdmissionStatusAsync(admissionProgram.ApplicantAdmissionId);
            if (!isSuccess) return false;

            await _admissionProgramRepository.DeleteAsync(admissionProgram);

            return true;
        }

        public async Task<bool> UpdateAdmissionProgramRangeAsync(List<AdmissionProgram> admissionPrograms, Guid admissionId)
        {
            bool isSuccess = await UpdateAdmissionStatusAsync(admissionId);
            if (!isSuccess) return false;

            await _admissionProgramRepository.UpdateRangeAsync(admissionPrograms);

            return true;
        }

        public async Task ChangeAdmissionStatusAsync(ApplicantAdmission applicantAdmission, ManagerChangeAdmissionStatus changeAdmissionStatus)
        {
            applicantAdmission.LastUpdate = DateTime.UtcNow;
            applicantAdmission.AdmissionStatus = (AdmissionStatus)changeAdmissionStatus;

            await _applicantAdmissionRepository.UpdateAsync(applicantAdmission);
        }

        private async Task<bool> UpdateAdmissionStatusAsync(Guid admissionId)
        {
            ApplicantAdmission? applicantAdmission = await _applicantAdmissionRepository.GetByIdAsync(admissionId);
            if (applicantAdmission is null) return false;

            UpdateAdmissionStatusFromRejectedAndConfirmed(applicantAdmission);

            await _applicantAdmissionRepository.UpdateAsync(applicantAdmission);

            return true;
        }

        private void UpdateAdmissionStatusFromRejectedAndConfirmed(ApplicantAdmission applicantAdmission)
        {
            applicantAdmission.LastUpdate = DateTime.UtcNow;

            AdmissionStatus[] rejectedOrConfirmed = [AdmissionStatus.Rejected, AdmissionStatus.Confirmed];
            if (rejectedOrConfirmed.Contains(applicantAdmission.AdmissionStatus))
            {
                applicantAdmission.AdmissionStatus 
                    = (applicantAdmission.ManagerId is null ? AdmissionStatus.Created : AdmissionStatus.UnderConsideration);
            }
        }
    }
}
/*
 
 К изменению статуса поступления ведет:
    1. +++ Обновление данных абитуриента (+++ данные пользователя и +++ абитуриента, который относится к этому пользователю (+++ User, +++ Applicant и (+++ Admission сервисы))
    2. +++ Добавление документов и +++ сканов абитуриента (+++ паспорт, +++ документы об образовании (+++ Applicant сервис))
    3. +++ Изменения статуса менеджером (+++ Менеджер отклонил поступление, +++ Менеджер принял поступление, +++ Менеджер закрывает поступление (+++ Admission сервис))
    4. +++ Добавление и удаление программ поступления (+++ Admission сервис)
    5. +++ Создание нового поступления (+++ Admission сервис)
    6. +++ Взятие абитуриента менеджером (+++ Admission сервис)
    7. +++ Отказ от абитуриента менеджером (+++ Admission сервис)
 */