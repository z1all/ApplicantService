using AdmissioningService.Core.Application.DTOs;
using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Application.Interfaces.Services;
using AdmissioningService.Core.Application.Interfaces.StateMachines;
using AdmissioningService.Core.Application.Mappers;
using AdmissioningService.Core.Application.Helpers;
using AdmissioningService.Core.Domain;
using Common.Models.Models;

namespace AdmissioningService.Core.Application.Services
{
    public class AdmissionService : IAdmissionService
    {
        private readonly IAdmissionCompanyRepository _companyRepository;
        private readonly IAdmissionProgramRepository _admissionProgramRepository;
        private readonly IApplicantAdmissionStateMachin _applicantAdmissionStateMachin;

        private readonly DictionaryHelper _dictionaryHelper;
        private readonly AdmissionHelper _admissionHelper;

        public AdmissionService(
            IAdmissionCompanyRepository companyRepository,
            IAdmissionProgramRepository admissionProgramRepository,
            IApplicantAdmissionStateMachin applicantAdmissionStateMachin,
            DictionaryHelper dictionaryHelper, AdmissionHelper admissionHelper)
        {
            _companyRepository = companyRepository;
            _admissionProgramRepository = admissionProgramRepository;
            _applicantAdmissionStateMachin = applicantAdmissionStateMachin;

            _dictionaryHelper = dictionaryHelper;
            _admissionHelper = admissionHelper;
        }

        public async Task<ExecutionResult<List<AdmissionCompanyDTO>>> GetAdmissionCompaniesAsync(Guid applicantId)
        {
            List<AdmissionCompany> admissionCompanies = await _companyRepository.GetAllWithApplicantAdmissionAsync(applicantId);

            return new()
            {
                Result = admissionCompanies
                            .Select(admissionCompany => admissionCompany.ToAdmissionCompanyDTO())
                            .ToList()
            };
        }

        public async Task<ExecutionResult> CreateAdmissionAsync(Guid applicantId)
        {
            AdmissionCompany? admissionCompany = await _companyRepository.GetCurrentAsync();
            if (admissionCompany is null)
            {
                return new(keyError: "NotExistCurrentAdmission", error: "There is no current admissions company at the moment. Try again later!");
            }

            ApplicantAdmission? applicantAdmission = await _applicantAdmissionStateMachin.GetByAdmissionCompanyIdAndApplicantId(admissionCompany.Id, applicantId);
            if (applicantAdmission is not null)
            {
                return new(keyError: "AdmissionAlreadyExist", error: "The applicant already has an admission in the current admission company!");
            }

            ExecutionResult checkingResult = await _admissionHelper.CheckApplicantAsync(applicantId);
            if (!checkingResult.IsSuccess) return new() { Errors = checkingResult.Errors };

            await _applicantAdmissionStateMachin.AddAsync(applicantId, admissionCompany);

            return new(isSuccess: true);
        }

        public async Task<ExecutionResult<ApplicantAdmissionDTO>> GetApplicantAdmissionAsync(Guid applicantId, Guid admissionId)
        {
            ApplicantAdmission? admission = await _applicantAdmissionStateMachin.GetByApplicantIdAndAdmissionIdAsync(applicantId, admissionId);
            if (admission is null)
            {
                return new(keyError: "AdmissionNotFound", error: $"Applicant with id {applicantId} doesn't have admission with id {admissionId}!");
            }

            List<AdmissionProgram> programs = await _admissionProgramRepository.GetAllByApplicantIdWithProgramWithLevelAndFacultyAsync(applicantId);

            return new()
            {
                Result = new()
                {
                    LastUpdate = admission.LastUpdate,
                    ExistManager = admission.ManagerId != null,
                    AdmissionCompany = admission.AdmissionCompany!.ToAdmissionCompanyDTO(),
                    AdmissionPrograms = programs.Select(program => program.ToAdmissionProgramDTO()).ToList()
                }
            };
        }

        public async Task<ExecutionResult> AddProgramToAdmissionAsync(Guid applicantId, Guid admissionId, Guid programId, Guid? managerId)
        {
            ExecutionResult canEdit = await _admissionHelper.CheckPermissionsAsync(applicantId, managerId);
            if (!canEdit.IsSuccess) return canEdit;

            ExecutionResult result = await _dictionaryHelper.CheckProgramAsync(programId);
            if (!result.IsSuccess) return result;

            bool admissionExist = await _applicantAdmissionStateMachin.AnyByApplicantIdAndAdmissionIdAsync(applicantId, admissionId);
            if (!admissionExist)
            {
                return new(keyError: "AdmissionNotFound", error: $"Applicant with id {applicantId} doesn't have admission with id {admissionId}!");
            }

            ExecutionResult<int> checkingResult = await _admissionHelper.CheckAdmissionProgramAsync(applicantId, admissionId, programId);
            if (!checkingResult.IsSuccess) return checkingResult;

            AdmissionProgram admissionProgram = new()
            {
                Priority = checkingResult.Result!,
                ApplicantAdmissionId = admissionId,
                EducationProgramId = programId,
            };

            await _admissionProgramRepository.AddAsync(admissionProgram);

            return new(isSuccess: true);
        }

        public Task<ExecutionResult> ChangeAdmissionProgramPriorityAsync(Guid applicantId, Guid admissionId, ChangePrioritiesApplicantProgramDTO changePriorities, Guid? managerId)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult> DeleteAdmissionProgramAsync(Guid applicantId, Guid admissionId, Guid programId, Guid? managerId)
        {
            throw new NotImplementedException();
        }
    }
}
