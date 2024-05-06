using AdmissioningService.Core.Domain;
using Common.Models.Enums;

namespace AdmissioningService.Core.Application.Interfaces.StateMachines
{
    /// <summary>
    /// Интерфейс собирает все методы, которые меняют информацию абитуриента.
    /// При изменении информации абитуриента в зависимости от метода меняется статус поступления абитуриента.
    /// </summary>
    public interface IApplicantAdmissionStateMachin
    {
        /// <summary>
        /// После создания поступления его статус становится Created 
        /// </summary>
        Task CreateApplicantAdmissionAsync(Guid applicantId, AdmissionCompany admissionCompany);

        /// <summary>
        /// После добавления менеджера, если статус поступления был Created, то становится UnderConsideration.
        /// Если же был другой статус, то ничего не меняется.
        /// </summary>
        Task AddManagerAsync(ApplicantAdmission applicantAdmission, Manager manager);

        /// <summary>
        /// После удаления менеджера, если статус поступления был UnderConsideration, то становится Created.
        /// Если же был другой статус, то ничего не меняется.
        /// </summary>
        Task DeleteManagerAsync(ApplicantAdmission applicantAdmission);

        /// <summary>
        /// После обновления данных абитуриент, если статус поступления был Confirmed или Rejected, то становится UnderConsideration, 
        /// если поступлению прикреплен менеджер, иначе - Created.
        /// Если же был другой статус, то ничего не меняется.
        /// </summary>
        Task<bool> ApplicantInfoUpdatedAsync(Guid applicantId);

        /// <summary>
        /// После добавления программы в поступление, если статус поступления был Confirmed или Rejected, то становится UnderConsideration, 
        /// если поступлению прикреплен менеджер, иначе - Created.
        /// Если же был другой статус, то ничего не меняется.
        /// </summary>
        Task<bool> AddAdmissionProgramAsync(Guid applicantId, AdmissionProgram admissionProgram);

        /// <summary>
        /// После удалении программы из поступления, если статус поступления был Confirmed или Rejected, то становится UnderConsideration, 
        /// если поступлению прикреплен менеджер, иначе - Created.
        /// Если же был другой статус, то ничего не меняется.
        /// </summary>
        Task<bool> DeleteAdmissionProgramAsync(Guid applicantId, AdmissionProgram admissionProgram);

        /// <summary>
        /// После изменении программ поступлений, если статус поступления был Confirmed или Rejected, то становится UnderConsideration, 
        /// если поступлению прикреплен менеджер, иначе - Created.
        /// Если же был другой статус, то ничего не меняется.
        /// </summary>
        Task<bool> UpdateAdmissionProgramRangeAsync(Guid applicantId, List<AdmissionProgram> admissionPrograms, Guid admissionId); 

        /// <summary>
        /// Меняет статус поступления абитуриента на Rejected, Confirmed или Closed.
        /// После изменения статуса на Closed абитуриент теряет возможность поменять свои данные, это может сделать только менеджер.
        /// </summary>
        Task ChangeAdmissionStatusAsync(ApplicantAdmission applicantAdmission, ManagerChangeAdmissionStatus changeAdmissionStatus);
    }
}