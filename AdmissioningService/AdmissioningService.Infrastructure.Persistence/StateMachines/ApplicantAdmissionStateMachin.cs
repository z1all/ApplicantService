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

        public async Task UpdateAsync(ApplicantAdmission entity, bool isUpdated = true)
        {
            await _applicantAdmissionRepository.UpdateAsync(entity);
        }
    }
}
/*
 
 К изменению статуса поступления ведет:
    1. Обновление данных абитуриента (данные пользователя и абитуриента, который относится к этому пользователю)
    2. Добавление документов и сканов абитуриента (паспорт, документы об образовании)
    3. Изменения статуса менеджером (Менеджер отклонил поступление, Менеджер принял поступление, Менеджер закрывает поступление)
    4. Добавление программ поступления
    5. Создание нового поступления
    6. Взятие абитуриента менеджером 
    7. Отказ от абитуриента менеджером 


Переделать отправку уведомлений об изменении абитуриента
 */