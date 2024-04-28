using Microsoft.Extensions.Options;
using AdmissioningService.Core.Application.Configurations;
using AdmissioningService.Core.Application.DTOs;
using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Application.Interfaces.Services;
using AdmissioningService.Core.Application.Interfaces.StateMachines;
using AdmissioningService.Core.Application.Mappers;
using AdmissioningService.Core.DictionaryHelpers;
using AdmissioningService.Core.Domain;
using Common.Models.Models;
using Common.ServiceBus.ServiceBusDTOs.FromApplicantService;

namespace AdmissioningService.Core.Application.Services
{
    public class AdmissionService : IAdmissionService
    {
        private readonly IAdmissionCompanyRepository _companyRepository;
        private readonly IApplicantCacheRepository _applicantCacheRepository;
        private readonly IAdmissionProgramRepository _admissionProgramRepository;
        private readonly IEducationProgramCacheRepository _educationProgramCacheRepository;
        private readonly IEducationDocumentTypeCacheRepository _educationDocumentTypeCacheRepository;
        private readonly IApplicantAdmissionStateMachin _applicantAdmissionStateMachin;
        private readonly IRequestService _requestService;

        private readonly DictionaryHelper _dictionaryHelper;
        private readonly AdmissionOptions _admissionOptions;

        public AdmissionService(
            IAdmissionCompanyRepository companyRepository, IApplicantCacheRepository applicantCacheRepository,
            IAdmissionProgramRepository admissionProgramRepository, IEducationProgramCacheRepository educationProgramCacheRepository,
            IEducationDocumentTypeCacheRepository educationDocumentTypeCacheRepository,
            IApplicantAdmissionStateMachin applicantAdmissionStateMachin,
            IRequestService requestService,
            DictionaryHelper dictionaryHelper, IOptions<AdmissionOptions> admissionOptions)
        {
            _companyRepository = companyRepository;
            _applicantCacheRepository = applicantCacheRepository;
            _admissionProgramRepository = admissionProgramRepository;
            _educationProgramCacheRepository = educationProgramCacheRepository;
            _educationDocumentTypeCacheRepository = educationDocumentTypeCacheRepository;
            _applicantAdmissionStateMachin = applicantAdmissionStateMachin;
            _requestService = requestService;

            _dictionaryHelper = dictionaryHelper;
            _admissionOptions = admissionOptions.Value;
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

            ApplicantAdmission? applicantAdmission = await _applicantAdmissionStateMachin.GetByAdmissionCompanyId(admissionCompany.Id);
            if (applicantAdmission is not null)
            {
                return new(keyError: "AdmissionAlreadyExist", error: "The applicant already has an admission in the current admission company!");
            }

            ExecutionResult checkingResult = await CheckApplicantAsync(applicantId);
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

        public async Task<ExecutionResult> AddProgramToAdmissionAsync(Guid applicantId, Guid admissionId, Guid programId)
        {
            ExecutionResult result = await _dictionaryHelper.CheckProgramAsync(programId);
            if (!result.IsSuccess) return result;

            bool admissionExist = await _applicantAdmissionStateMachin.AnyByApplicantIdAndAdmissionIdAsync(applicantId, admissionId);
            if (!admissionExist)
            {
                return new(keyError: "AdmissionNotFound", error: $"Applicant with id {applicantId} doesn't have admission with id {admissionId}!");
            }

            ExecutionResult<int> checkingResult = await CheckAdmissionProgramAsync(applicantId, admissionId, programId);
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

        public Task<ExecutionResult> ChangeAdmissionProgramPriorityAsync(Guid applicantId, Guid admissionId, ChangePrioritiesApplicantProgramDTO changePriorities)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult> DeleteAdmissionProgramAsync(Guid applicantId, Guid admissionId, Guid programId)
        {
            throw new NotImplementedException();
        }

        private async Task<ExecutionResult<int>> CheckAdmissionProgramAsync(Guid applicantId, Guid admissionId, Guid programId)
        {
            List<AdmissionProgram> admissionProgramsCache = await _admissionProgramRepository.GetAllByAdmissionIdAsync(admissionId);

            int currentCountPrograms = admissionProgramsCache.Count();
            if (currentCountPrograms >= _admissionOptions.MaxCountAdmissionPrograms)
            {
                return new(keyError: "MaxCountAdmissionPrograms", error: $"An applicant can have a maximum of {_admissionOptions.MaxCountAdmissionPrograms} admission programs");
            }

            EducationProgramCache? program = await _educationProgramCacheRepository.GetByIdWithoutLevelAsync(programId);
            if (program is null)
            {
                return new(keyError: "EducationProgramNotFound", error: $"Education program with id {programId} not found!");
            }

            ExecutionResult checkingExistRightDocumentTypeResult = await CheckExistRightDocumentTypeAsync(applicantId, program.EducationLevelId);
            if (!checkingExistRightDocumentTypeResult.IsSuccess) return new() { Errors = checkingExistRightDocumentTypeResult.Errors };

            if(admissionProgramsCache.Count() > 0)
            {
                Guid firstProgramsId = admissionProgramsCache.First().EducationProgramId;

                ExecutionResult checkingDocumentTypesOnCommonStageResult = await CheckDocumentTypesOnCommonStageAsync(firstProgramsId, program.EducationLevelId);
                if (!checkingDocumentTypesOnCommonStageResult.IsSuccess) return new() { Errors = checkingDocumentTypesOnCommonStageResult.Errors };
            }

            bool programAlreadyExist = admissionProgramsCache.Any(admissionProgram => admissionProgram.EducationProgramId == programId);
            if (programAlreadyExist)
            {
                return new(keyError: "ProgramAlreadyExist", error: $"An applicant already have education program with id {programId}!");
            }

            AdmissionProgram? lastPriority = admissionProgramsCache.OrderBy(admissionProgram => admissionProgram.Priority).LastOrDefault();

            return new()
            {
                Result = lastPriority?.Priority + 1 ?? 0,
            };
        }

        private async Task<ExecutionResult> CheckExistRightDocumentTypeAsync(Guid applicantId, Guid addingDocumentTypeLevelId)
        {
            ApplicantCache? applicant = await _applicantCacheRepository.GetByIdWithDocumentTypeAndLevelsAsync(applicantId);
            if (applicant is null)
            {
                return new(keyError: "ApplicantNotFound", error: $"Applicant with id {applicantId} not found!");
            }

            foreach (var addedDocumentType in applicant.AddedDocumentTypes)
            {
                if (addedDocumentType.EducationLevelId == addingDocumentTypeLevelId)
                {
                    return new(isSuccess: true);
                }

                foreach(var nextLevel in addedDocumentType.NextEducationLevel)
                {
                    if(nextLevel.Id == addingDocumentTypeLevelId)
                    {
                        return new(isSuccess: true);
                    }
                }
            }

            return new(keyError: "NoRequiredDocument", error: "There is no required document!");
        }

        private async Task<ExecutionResult> CheckDocumentTypesOnCommonStageAsync(Guid firstProgramsId, Guid addingDocumentTypeLevelId)
        {
            EducationProgramCache? program = await _educationProgramCacheRepository.GetByIdWithLevelAsync(firstProgramsId);
            if(program is null)
            {
                return new(keyError: "EducationProgramNotFound", error: $"Education program with id {firstProgramsId} not found");
            }

            EducationLevelCache level = program.EducationLevel!;
            List<EducationDocumentTypeCache>? documentTypes = await _educationDocumentTypeCacheRepository.GetAllByNextEducationLevelWithNextLevelId(level.Id);
            foreach(var documentType in documentTypes)
            {
                foreach(var nextLevel in documentType.NextEducationLevel)
                {
                    if (nextLevel.Id == addingDocumentTypeLevelId)
                    {
                        return new(isSuccess: true);
                    }
                }
            }
            
            return new(keyError: "DocumentTypeNotOnCommonStage", error: $"Programs from different stages!");
        }

        private async Task<ExecutionResult> CheckApplicantAsync(Guid applicantId)
        {
            ApplicantCache? applicant = await _applicantCacheRepository.GetByIdAsync(applicantId);
            if (applicant is null)
            {
                ExecutionResult<GetApplicantResponse> result = await _requestService.GetApplicantAsync(applicantId);
                if (!result.IsSuccess) return new() { Errors = result.Errors };
                GetApplicantResponse applicantResponse = result.Result!;

                applicant = new()
                {
                    Id = applicantResponse.Id,
                    Email = applicantResponse.Email,
                    FullName = applicantResponse.FullName,
                    AddedDocumentTypes = await _dictionaryHelper.ToDocumentTypeFromDbAsync(applicantResponse.AddedDocumentTypesId)
                };

                await _applicantCacheRepository.AddAsync(applicant);
            }

            return new(isSuccess: true);
        }
    }
}
