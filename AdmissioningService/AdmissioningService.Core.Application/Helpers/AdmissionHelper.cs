using Microsoft.Extensions.Options;
using AdmissioningService.Core.Application.Configurations;
using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Application.Interfaces.Services;
using AdmissioningService.Core.Application.Interfaces.StateMachines;
using AdmissioningService.Core.Domain;
using Common.Models.Models;
using Common.ServiceBus.ServiceBusDTOs.FromApplicantService;

namespace AdmissioningService.Core.Application.Helpers
{
    public class AdmissionHelper
    {
        private readonly IApplicantCacheRepository _applicantCacheRepository;
        private readonly IAdmissionProgramRepository _admissionProgramRepository;
        private readonly IEducationProgramCacheRepository _educationProgramCacheRepository;
        private readonly IEducationDocumentTypeCacheRepository _educationDocumentTypeCacheRepository;
        private readonly IApplicantAdmissionStateMachin _applicantAdmissionStateMachin;
        private readonly IRequestService _requestService;

        private readonly AdmissionOptions _admissionOptions;
        private readonly DictionaryHelper _dictionaryHelper;

        public AdmissionHelper(
            IAdmissionProgramRepository admissionProgramRepository, IOptions<AdmissionOptions> admissionOptions,
            IEducationProgramCacheRepository educationProgramCacheRepository, DictionaryHelper dictionaryHelper,
            IApplicantCacheRepository applicantCacheRepository, IApplicantAdmissionStateMachin applicantAdmissionStateMachin,
            IEducationDocumentTypeCacheRepository educationDocumentTypeCacheRepository, IRequestService requestService)
        {
            _applicantCacheRepository = applicantCacheRepository;
            _admissionProgramRepository = admissionProgramRepository;
            _educationProgramCacheRepository = educationProgramCacheRepository;
            _educationDocumentTypeCacheRepository = educationDocumentTypeCacheRepository;
            _applicantAdmissionStateMachin = applicantAdmissionStateMachin;
            _requestService = requestService;

            _admissionOptions = admissionOptions.Value;
            _dictionaryHelper = dictionaryHelper;
        }

        public async Task<ExecutionResult> CheckPermissionsAsync(Guid applicantId, Guid? managerId)
        {
            if (managerId is null)
            {
                bool isNotClose = await _applicantAdmissionStateMachin.CheckAdmissionStatusIsCloseAsync(applicantId);
                if (isNotClose) return new(isSuccess: true);
                return new(keyError: "AdmissionClosed", error: "You cannot change the data when the current admission is closed!");
            }

            bool managerCanEdit = await _applicantAdmissionStateMachin.CheckManagerEditPermissionAsync(applicantId, (Guid)managerId);
            if (managerCanEdit) return new(isSuccess: true);
            return new(keyError: "NoEditPermission", error: $"Manager with id {managerId} doesn't have permission to edit applicant with id {applicantId}");
        }

        public async Task<ExecutionResult> CheckApplicantAsync(Guid applicantId)
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

        public async Task<ExecutionResult<int>> CheckAdmissionProgramAsync(Guid applicantId, Guid admissionId, Guid programId)
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

            bool programAlreadyExist = admissionProgramsCache.Any(admissionProgram => admissionProgram.EducationProgramId == programId);
            if (programAlreadyExist)
            {
                return new(keyError: "ProgramAlreadyExist", error: $"An applicant already have education program with id {programId}!");
            }

            ExecutionResult checkingExistRightDocumentTypeResult = await CheckExistRightDocumentTypeAsync(applicantId, program.EducationLevelId);
            if (!checkingExistRightDocumentTypeResult.IsSuccess) return new() { Errors = checkingExistRightDocumentTypeResult.Errors };

            if (admissionProgramsCache.Count() > 0)
            {
                Guid firstProgramsId = admissionProgramsCache.First().EducationProgramId;

                ExecutionResult checkingDocumentTypesOnCommonStageResult = await CheckDocumentTypesOnCommonStageAsync(firstProgramsId, program.EducationLevelId);
                if (!checkingDocumentTypesOnCommonStageResult.IsSuccess) return new() { Errors = checkingDocumentTypesOnCommonStageResult.Errors };
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

                foreach (var nextLevel in addedDocumentType.NextEducationLevel)
                {
                    if (nextLevel.Id == addingDocumentTypeLevelId)
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
            if (program is null)
            {
                return new(keyError: "EducationProgramNotFound", error: $"Education program with id {firstProgramsId} not found");
            }

            EducationLevelCache level = program.EducationLevel!;
            List<EducationDocumentTypeCache>? documentTypes = await _educationDocumentTypeCacheRepository.GetAllByNextEducationLevelWithNextLevelId(level.Id);
            foreach (var documentType in documentTypes)
            {
                foreach (var nextLevel in documentType.NextEducationLevel)
                {
                    if (nextLevel.Id == addingDocumentTypeLevelId)
                    {
                        return new(isSuccess: true);
                    }
                }
            }

            return new(keyError: "DocumentTypeNotOnCommonStage", error: $"Programs from different stages!");
        }
    }
}
