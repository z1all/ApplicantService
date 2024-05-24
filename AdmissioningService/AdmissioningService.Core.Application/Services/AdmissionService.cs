using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Application.Interfaces.Services;
using AdmissioningService.Core.Application.Interfaces.StateMachines;
using AdmissioningService.Core.Application.Mappers;
using AdmissioningService.Core.Application.Helpers;
using AdmissioningService.Core.Domain;
using Common.Models.Models;
using Common.Models.DTOs.Admission;

namespace AdmissioningService.Core.Application.Services
{
    public class AdmissionService : IAdmissionService
    {
        private readonly IAdmissionCompanyRepository _companyRepository;
        private readonly IAdmissionProgramRepository _admissionProgramRepository;
        private readonly IApplicantAdmissionRepository _applicantAdmissionRepository;
        private readonly IApplicantAdmissionStateMachin _applicantAdmissionStateMachin;

        private readonly DictionaryHelper _dictionaryHelper;
        private readonly AdmissionHelper _admissionHelper;

        public AdmissionService(
            IAdmissionCompanyRepository companyRepository,
            IAdmissionProgramRepository admissionProgramRepository,
            IApplicantAdmissionRepository applicantAdmissionRepository,
            IApplicantAdmissionStateMachin applicantAdmissionStateMachin,
            DictionaryHelper dictionaryHelper, AdmissionHelper admissionHelper)
        {
            _companyRepository = companyRepository;
            _admissionProgramRepository = admissionProgramRepository;
            _applicantAdmissionRepository = applicantAdmissionRepository;
            _applicantAdmissionStateMachin = applicantAdmissionStateMachin;

            _dictionaryHelper = dictionaryHelper;
            _admissionHelper = admissionHelper;
        }

        public async Task<ExecutionResult<List<AdmissionCompanyDTO>>> GetAdmissionCompaniesAsync()
        {
            List<AdmissionCompany> admissionCompanies = await _companyRepository.GetAllAsync();

            return new()
            {
                Result = admissionCompanies
                            .Select(admissionCompany => admissionCompany.ToAdmissionCompanyDTO())
                            .ToList()
            };
        }

        public async Task<ExecutionResult<List<AdmissionCompanyDTO>>> GetAdmissionCompaniesWithApplicantAdmissionsAsync(Guid applicantId)
        {
            List<AdmissionCompany> admissionCompanies = await _companyRepository.GetAllWithApplicantAdmissionAsync(applicantId);

            return new()
            {
                Result = admissionCompanies
                            .Select(admissionCompany => admissionCompany.ToAdmissionCompanyDTO())
                            .ToList()
            };
        }

        public async Task<ExecutionResult> CreateAdmissionCompanyAsync(int year)
        {
            AdmissionCompany? currentCompany = await _companyRepository.GetCurrentAsync();
            if (currentCompany is not null)
            {
                currentCompany.IsCurrent = false;
                await _companyRepository.UpdateAsync(currentCompany);
            }

            AdmissionCompany newCompany = new()
            {
                IsCurrent = true,
                EventYear = year,
            };
            await _companyRepository.AddAsync(newCompany);

            return new(isSuccess: true);
        }

        public async Task<ExecutionResult> CreateAdmissionAsync(Guid applicantId)
        {
            AdmissionCompany? admissionCompany = await _companyRepository.GetCurrentAsync();
            if (admissionCompany is null)
            {
                return new(StatusCodeExecutionResult.BadRequest, keyError: "NotExistCurrentAdmission", error: "There is no current admissions company at the moment. Try again later!");
            }

            ApplicantAdmission? applicantAdmission = await _applicantAdmissionRepository.GetByAdmissionCompanyIdAndApplicantId(admissionCompany.Id, applicantId);
            if (applicantAdmission is not null)
            {
                return new(StatusCodeExecutionResult.BadRequest, keyError: "AdmissionAlreadyExist", error: "The applicant already has an admission in the current admission company!");
            }

            ExecutionResult checkingResult = await _admissionHelper.CheckApplicantAsync(applicantId);
            if (!checkingResult.IsSuccess) return new(checkingResult.StatusCode, errors: checkingResult.Errors);

            await _applicantAdmissionStateMachin.CreateApplicantAdmissionAsync(applicantId, admissionCompany);

            return new(isSuccess: true);
        }

        public async Task<ExecutionResult<ApplicantAdmissionDTO>> GetApplicantAdmissionAsync(Guid applicantId, Guid admissionId)
        {
            ApplicantAdmission? admission = await _applicantAdmissionRepository.GetByApplicantIdAndAdmissionIdAsync(applicantId, admissionId);
            if (admission is null)
            {
                return new(StatusCodeExecutionResult.NotFound, keyError: "AdmissionNotFound", error: $"Applicant with id {applicantId} doesn't have admission with id {admissionId}!");
            }

            List<AdmissionProgram> programs = await _admissionProgramRepository.GetAllByApplicantIdAndAdmissionIdWithProgramWithLevelAndFacultyOrderByPriorityAsync(applicantId, admissionId);
            return new()
            {
                Result = admission.ToApplicantAdmissionDTO(programs)
            };
        }

        public async Task<ExecutionResult> AddProgramToCurrentAdmissionAsync(Guid applicantId, Guid programId, Guid? managerId)
        {
            ExecutionResult canEdit = await _admissionHelper.CheckPermissionsAsync(applicantId, managerId);
            if (!canEdit.IsSuccess) return canEdit;

            ExecutionResult result = await _dictionaryHelper.CheckProgramAsync(programId);
            if (!result.IsSuccess) return result;

            ApplicantAdmission? applicantAdmission = await _applicantAdmissionRepository.GetCurrentByApplicantIdAsync(applicantId);
            if (applicantAdmission is null)
            {
                return new(StatusCodeExecutionResult.NotFound, keyError: "AdmissionNotFound", error: $"Applicant with id {applicantId} doesn't have admission in current company!");
            }

            ExecutionResult<int> checkingResult = await _admissionHelper.CheckAdmissionProgramAsync(applicantId, applicantAdmission.Id, programId);
            if (!checkingResult.IsSuccess) return checkingResult;

            AdmissionProgram admissionProgram = new()
            {
                Priority = checkingResult.Result!,
                ApplicantAdmissionId = applicantAdmission.Id,
                EducationProgramId = programId,
            };

            bool isSuccess = await _applicantAdmissionStateMachin.AddAdmissionProgramAsync(applicantId, admissionProgram);
            if (!isSuccess)
            {
                return new(StatusCodeExecutionResult.BadRequest, keyError: "AddAdmissionProgramFail", error: "Unknown error.");
            }

            return new(isSuccess: true);
        }

        public async Task<ExecutionResult> ChangeAdmissionProgramPriorityAsync(Guid applicantId, ChangePrioritiesApplicantProgramDTO changePriorities, Guid? managerId)
        {
            ExecutionResult canEdit = await _admissionHelper.CheckPermissionsAsync(applicantId, managerId);
            if (!canEdit.IsSuccess) return canEdit;

            ApplicantAdmission? applicantAdmission = await _applicantAdmissionRepository.GetCurrentByApplicantIdAsync(applicantId);
            if (applicantAdmission is null)
            {
                return new(StatusCodeExecutionResult.NotFound, keyError: "AdmissionNotFound", error: $"Applicant with id {applicantId} doesn't have admission in current company!");
            }

            List<AdmissionProgram> admissionPrograms = await _admissionProgramRepository.GetAllByAdmissionIdWithOrderByPriorityAsync(applicantAdmission.Id);
            if (admissionPrograms.Count != changePriorities.NewProgramPrioritiesOrder.Count)
            {
                return new(StatusCodeExecutionResult.BadRequest, keyError: "WrongProgramCount", error: $"There are {admissionPrograms.Count} programs in the applicant's admission!");
            }

            ExecutionResult<List<AdmissionProgram>> result = _admissionHelper.GetNewProgramsOrder(changePriorities.NewProgramPrioritiesOrder, admissionPrograms);
            if (!result.IsSuccess) return new(result.StatusCode, errors: result.Errors);
            List<AdmissionProgram> newAdmissionProgramsPriorities = result.Result!;

            bool isSuccess = await _applicantAdmissionStateMachin.UpdateAdmissionProgramRangeAsync(applicantId, newAdmissionProgramsPriorities, applicantAdmission.Id);
            if (!isSuccess)
            {
                return new(StatusCodeExecutionResult.BadRequest, keyError: "ChangeAdmissionProgramPriorityFail", error: $"Unknown error");
            }

            return new(isSuccess: true);
        }

        public async Task<ExecutionResult> DeleteAdmissionProgramAsync(Guid applicantId, Guid programId, Guid? managerId)
        {
            ExecutionResult canEdit = await _admissionHelper.CheckPermissionsAsync(applicantId, managerId);
            if (!canEdit.IsSuccess) return canEdit;

            ApplicantAdmission? applicantAdmission = await _applicantAdmissionRepository.GetCurrentByApplicantIdAsync(applicantId);
            if (applicantAdmission is null)
            {
                return new(StatusCodeExecutionResult.NotFound, keyError: "AdmissionNotFound", error: $"Applicant with id {applicantId} doesn't have admission in current company!");
            }

            var (admissionProgramForDelete, newAdmissionProgramsPriorities) = await _admissionHelper.GetProgramForDeleteAndNewProgramsOrderAsync(applicantAdmission.Id, programId);
            if (admissionProgramForDelete is null)
            {
                return new(StatusCodeExecutionResult.NotFound, keyError: "ProgramNotFound", error: $"Applicant with id {applicantId} in current admission doesn't have program with id {programId}!");
            }

            bool deleteIsSuccess = await _applicantAdmissionStateMachin.DeleteAdmissionProgramAsync(applicantId, admissionProgramForDelete);
            if (!deleteIsSuccess)
            {
                return new(StatusCodeExecutionResult.BadRequest, keyError: "DeleteAdmissionProgramFail", error: $"Unknown error");
            }

            bool updateIsSuccess = await _applicantAdmissionStateMachin.UpdateAdmissionProgramRangeAsync(applicantId, newAdmissionProgramsPriorities, applicantAdmission.Id);
            if (!updateIsSuccess)
            {
                return new(StatusCodeExecutionResult.BadRequest, keyError: "DeleteAdmissionProgramFail", error: $"Unknown error");
            }

            return new(isSuccess: true);
        }

        public async Task<ExecutionResult<ApplicantAdmissionPagedDTO>> GetApplicantAdmissionsAsync(ApplicantAdmissionFilterDTO filter, Guid managerId)
        {
            if (filter.Page < 1)
            {
                return new(StatusCodeExecutionResult.BadRequest, keyError: "InvalidPageError", error: "Number of page can't be less than 1.");
            }

            int countApplicantAdmission = await _applicantAdmissionRepository.CountAllAsync(filter, managerId);
            countApplicantAdmission = countApplicantAdmission == 0 ? 1 : countApplicantAdmission;

            int countPage = (countApplicantAdmission / filter.Size) + (countApplicantAdmission % filter.Size == 0 ? 0 : 1);
            if (filter.Page > countPage)
            {
                return new(StatusCodeExecutionResult.BadRequest, keyError: "InvalidPageError", error: $"Number of page can be from 1 to {countPage}.");
            }

            List<ApplicantAdmission> applicantAdmissions = await _applicantAdmissionRepository.GetAllByFiltersWithCompanyAndProgramsAsync(filter, managerId);
            return new()
            {
                Result = new()
                {
                    ApplicantAdmissions = applicantAdmissions.Select(applicantAdmission => applicantAdmission.ToApplicantAdmissionShortInfoDTO()).ToList(),
                    Pagination = new()
                    {
                        Count = countPage,
                        Current = filter.Page,
                        Size = filter.Size,
                    }
                }
            };
        }
    }
}