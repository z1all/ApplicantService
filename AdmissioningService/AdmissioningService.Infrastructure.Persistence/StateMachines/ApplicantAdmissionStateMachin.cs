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
            throw new NotImplementedException("Нужно реализовать машину состояний");
            
            ApplicantAdmission applicantAdmission = new()
            {
                LastUpdate = DateTime.UtcNow,
                AdmissionStatus = Common.Models.Enums.AdmissionStatus.Created,
                ApplicantId = applicantId,
                AdmissionCompanyId = admissionCompany.Id
            };

            await _applicantAdmissionRepository.AddAsync(applicantAdmission);
        }

        public async Task AddManagerAsync(ApplicantAdmission applicantAdmission, Manager manager)
        {
            throw new NotImplementedException("Нужно реализовать машину состояний");
            
            applicantAdmission.Manager = manager;

            await _applicantAdmissionRepository.UpdateAsync(applicantAdmission);
        }

        public async Task DeleteManagerAsync(ApplicantAdmission applicantAdmission)
        {
            throw new NotImplementedException("Нужно реализовать машину состояний");
            
            applicantAdmission.ManagerId = null;

            await _applicantAdmissionRepository.UpdateAsync(applicantAdmission);
        }

        public async Task ApplicantInfoUpdatedAsync(Guid applicantId)
        {
            throw new NotImplementedException("Нужно реализовать машину состояний");
            
            ApplicantAdmission? applicantAdmission = await _applicantAdmissionRepository.GetCurrentByApplicantIdAsync(applicantId);
            if (applicantAdmission is null) return;

            applicantAdmission.LastUpdate = DateTime.UtcNow;
        }

        public async Task AddAdmissionProgramAsync(AdmissionProgram admissionProgram)
        {
            throw new NotImplementedException("Нужно реализовать машину состояний");
            
            await _admissionProgramRepository.AddAsync(admissionProgram);
        }

        public async Task DeleteAdmissionProgramAsync(AdmissionProgram admissionProgram)
        {
            throw new NotImplementedException("Нужно реализовать машину состояний");
            
            await _admissionProgramRepository.DeleteAsync(admissionProgram);
        }

        public async Task UpdateAdmissionProgramRangeAsync(List<AdmissionProgram> admissionPrograms)
        {
            throw new NotImplementedException("Нужно реализовать машину состояний");
            
            await _admissionProgramRepository.UpdateRangeAsync(admissionPrograms);
        }

        public async Task ChangeAdmissionStatusAsync(ApplicantAdmission applicantAdmission, ManagerChangeAdmissionStatus changeAdmissionStatus)
        {
            throw new NotImplementedException("Нужно реализовать машину состояний");

            applicantAdmission.AdmissionStatus = (AdmissionStatus)changeAdmissionStatus;

            await _applicantAdmissionRepository.UpdateAsync(applicantAdmission);
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