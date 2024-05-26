using Microsoft.Extensions.Options;
using AdmissioningService.Core.Application.Configurations;
using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Application.Interfaces.Services;
using AdmissioningService.Core.Domain;
using Common.Models.Models;
using Common.ServiceBus.ServiceBusDTOs.FromApplicantService.Requests;

namespace AdmissioningService.Core.Application.Helpers
{
    public class AdmissionHelper
    {
        private readonly IManagerRepository _managerRepository;
        private readonly IApplicantCacheRepository _applicantCacheRepository;
        private readonly IAdmissionProgramRepository _admissionProgramRepository;
        private readonly IEducationProgramCacheRepository _educationProgramCacheRepository;
        private readonly IEducationDocumentTypeCacheRepository _educationDocumentTypeCacheRepository;
        private readonly IApplicantAdmissionRepository _applicantAdmissionRepository;
        private readonly IRequestService _requestService;

        private readonly AdmissionOptions _admissionOptions;
        private readonly DictionaryHelper _dictionaryHelper;

        public AdmissionHelper(
            IManagerRepository managerRepository,
            IAdmissionProgramRepository admissionProgramRepository, IOptions<AdmissionOptions> admissionOptions,
            IEducationProgramCacheRepository educationProgramCacheRepository, DictionaryHelper dictionaryHelper,
            IApplicantCacheRepository applicantCacheRepository, IApplicantAdmissionRepository applicantAdmissionRepository,
            IEducationDocumentTypeCacheRepository educationDocumentTypeCacheRepository, IRequestService requestService)
        {
            _managerRepository = managerRepository;
            _applicantCacheRepository = applicantCacheRepository;
            _admissionProgramRepository = admissionProgramRepository;
            _educationProgramCacheRepository = educationProgramCacheRepository;
            _educationDocumentTypeCacheRepository = educationDocumentTypeCacheRepository;
            _applicantAdmissionRepository = applicantAdmissionRepository;
            _requestService = requestService;

            _admissionOptions = admissionOptions.Value;
            _dictionaryHelper = dictionaryHelper;
            _applicantAdmissionRepository = applicantAdmissionRepository;
        }

        #region CheckPermissionsAsync

        public async Task<ExecutionResult> CheckPermissionsAsync(Guid applicantId, Guid? managerId)
        {
            if (managerId is null)
            {
                bool isNotClose = await CheckAdmissionStatusIsCloseAsync(applicantId);
                if (isNotClose) return new(isSuccess: true);
                return new(StatusCodeExecutionResult.Forbid, keyError: "AdmissionClosed", error: "You cannot change the data when the current admission is closed!");
            }

            bool managerCanEdit = await CheckManagerEditPermissionAsync(applicantId, (Guid)managerId);
            if (managerCanEdit) return new(isSuccess: true);
            return new(StatusCodeExecutionResult.Forbid, keyError: "NoEditPermission", error: $"Manager with id {managerId} doesn't have permission to edit applicant with id {applicantId}");
        }

        //private async Task<bool> CheckManagerEditPermissionAsync(Guid applicantId, Guid managerId)
        //{
        //    ApplicantAdmission? applicantAdmission = await _applicantAdmissionRepository.GetCurrentByApplicantIdAsync(applicantId);
        //    if (applicantAdmission is null) return true;

        //    Manager? manager = await _managerRepository.GetByIdAsync(managerId);

        //    if (manager is not null && manager.FacultyId is null || 
        //        applicantAdmission.ManagerId == managerId) return true;
        //    return false;
        //}

        private async Task<bool> CheckManagerEditPermissionAsync(Guid applicantId, Guid managerId)
        {
            ApplicantAdmission? applicantAdmission = await _applicantAdmissionRepository.GetCurrentByApplicantIdWithProgramsAsync(applicantId);
            if (applicantAdmission is null) return true;

            Manager? manager = await _managerRepository.GetByIdAsync(managerId);

            if (manager is not null && manager.FacultyId is null ||
                applicantAdmission.ManagerId == managerId)
            {
                return true;
            }

            AdmissionProgram? firstProgram = applicantAdmission.AdmissionPrograms.OrderBy(program => program.Priority).FirstOrDefault();
            if (firstProgram is null) return false;

            if (firstProgram.EducationProgram!.FacultyId == manager!.FacultyId)
            {
                return true;
            }
            return false;
        }

        private async Task<bool> CheckAdmissionStatusIsCloseAsync(Guid applicantId)
        {
            ApplicantAdmission? applicantAdmission = await _applicantAdmissionRepository.GetCurrentByApplicantIdAsync(applicantId);
            if (applicantAdmission is null) return true;

            if (applicantAdmission.AdmissionStatus != Common.Models.Enums.AdmissionStatus.Closed) return true;
            return false;
        }


        #endregion

        #region CheckApplicantAsync

        public async Task<ExecutionResult> CheckApplicantAsync(Guid applicantId)
        {
            ApplicantCache? applicant = await _applicantCacheRepository.GetByIdAsync(applicantId);
            if (applicant is null)
            {
                ExecutionResult<GetApplicantResponse> result = await _requestService.GetApplicantAsync(applicantId);
                if (!result.IsSuccess) return new(result.StatusCode, errors: result.Errors);
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

        #endregion

        #region CheckAdmissionProgramAsync

        public async Task<ExecutionResult<int>> CheckAdmissionProgramAsync(Guid applicantId, Guid admissionId, Guid programId)
        {
            List<AdmissionProgram> admissionProgramsCache = await _admissionProgramRepository.GetAllByAdmissionIdWithOrderByPriorityAsync(admissionId);

            int currentCountPrograms = admissionProgramsCache.Count;
            if (currentCountPrograms >= _admissionOptions.MaxCountAdmissionPrograms)
            {
                return new(StatusCodeExecutionResult.BadRequest, keyError: "MaxCountAdmissionPrograms", error: $"An applicant can have a maximum of {_admissionOptions.MaxCountAdmissionPrograms} admission programs");
            }

            EducationProgramCache? program = await _educationProgramCacheRepository.GetByIdWithoutLevelAsync(programId);
            if (program is null)
            {
                return new(StatusCodeExecutionResult.NotFound, keyError: "EducationProgramNotFound", error: $"Education program with id {programId} not found!");
            }

            bool programAlreadyExist = admissionProgramsCache.Any(admissionProgram => admissionProgram.EducationProgramId == programId);
            if (programAlreadyExist)
            {
                return new(StatusCodeExecutionResult.BadRequest, keyError: "ProgramAlreadyExist", error: $"An applicant already have education program with id {programId}!");
            }

            ExecutionResult checkingExistRightDocumentTypeResult = await CheckExistRightDocumentTypeAsync(applicantId, program.EducationLevelId);
            if (!checkingExistRightDocumentTypeResult.IsSuccess)
            {
                return new(checkingExistRightDocumentTypeResult.StatusCode, errors: checkingExistRightDocumentTypeResult.Errors);
            }

            if (admissionProgramsCache.Count > 0)
            {
                Guid firstProgramsId = admissionProgramsCache.First().EducationProgramId;

                ExecutionResult checkingDocumentTypesOnCommonStageResult = await CheckDocumentTypesOnCommonStageAsync(firstProgramsId, program.EducationLevelId);
                if (!checkingDocumentTypesOnCommonStageResult.IsSuccess)
                {
                    return new(checkingDocumentTypesOnCommonStageResult.StatusCode, errors: checkingDocumentTypesOnCommonStageResult.Errors);
                }
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
                return new(StatusCodeExecutionResult.NotFound, keyError: "ApplicantNotFound", error: $"Applicant with id {applicantId} not found!");
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

            return new(StatusCodeExecutionResult.BadRequest, keyError: "NoRequiredDocument", error: "There is no required document!");
        }

        private async Task<ExecutionResult> CheckDocumentTypesOnCommonStageAsync(Guid firstProgramsId, Guid addingDocumentTypeLevelId)
        {
            EducationProgramCache? program = await _educationProgramCacheRepository.GetByIdWithLevelAsync(firstProgramsId);
            if (program is null)
            {
                return new(StatusCodeExecutionResult.NotFound, keyError: "EducationProgramNotFound", error: $"Education program with id {firstProgramsId} not found");
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

            return new(StatusCodeExecutionResult.BadRequest, keyError: "DocumentTypeNotOnCommonStage", error: $"Programs from different stages!");
        }

        #endregion

        #region GetProgramForDeleteAndNewProgramsOrderAsync

        public async Task<Tuple<AdmissionProgram?, List<AdmissionProgram>>> GetProgramForDeleteAndNewProgramsOrderAsync(Guid admissionId, Guid programId)
        {
            List<AdmissionProgram> admissionPrograms = await _admissionProgramRepository.GetAllByAdmissionIdWithOrderByPriorityAsync(admissionId);

            AdmissionProgram? admissionProgramForDelete = null;
            List<AdmissionProgram> newAdmissionProgramsPriorities = [];
            for (int i = 0; i < admissionPrograms.Count; ++i)
            {
                AdmissionProgram admissionProgram = admissionPrograms[i];

                if (admissionProgramForDelete is null)
                {
                    if (admissionProgram.EducationProgramId == programId)
                    {
                        admissionProgramForDelete = admissionProgram;
                    }
                    else
                    {
                        admissionProgram.Priority = i;
                        newAdmissionProgramsPriorities.Add(admissionProgram);
                    }
                }
                else
                {
                    admissionProgram.Priority = i - 1;
                    newAdmissionProgramsPriorities.Add(admissionProgram);
                }
            }

            return new(admissionProgramForDelete, newAdmissionProgramsPriorities);
        }

        #endregion

        #region GetNewProgramsOrder

        public ExecutionResult<List<AdmissionProgram>> GetNewProgramsOrder(List<Guid> newProgramPrioritiesOrder, List<AdmissionProgram> admissionPrograms)
        {
            ExecutionResult result = CheckDuplicate(newProgramPrioritiesOrder);
            if (!result.IsSuccess) return new(result.StatusCode, errors: result.Errors);

            List<string> comments = [];
            List<AdmissionProgram> newAdmissionProgramsPriorities = [];
            for (int i = 0; i < newProgramPrioritiesOrder.Count; ++i)
            {
                Guid newProgramPriorityOrderId = newProgramPrioritiesOrder[i];
                AdmissionProgram? admissionProgram = admissionPrograms.FirstOrDefault(admissionProgram => admissionProgram.EducationProgramId == newProgramPriorityOrderId);
                if (admissionProgram is null)
                {
                    comments.Add($"Program with id {newProgramPriorityOrderId} not found!");
                    continue;
                }

                if (admissionProgram.Priority != i)
                {
                    admissionProgram.Priority = i;
                    newAdmissionProgramsPriorities.Add(admissionProgram);
                }
            }

            if (comments.Count > 0)
            {
                return new(StatusCodeExecutionResult.NotFound, keyError: "ProgramNotFound", error: comments.ToArray());
            }

            return new() { Result = newAdmissionProgramsPriorities };
        }

        private ExecutionResult CheckDuplicate(List<Guid> newProgramPrioritiesOrder)
        {
            for (int i = 0; i < newProgramPrioritiesOrder.Count; ++i)
            {
                for (int j = i + 1; j < newProgramPrioritiesOrder.Count; ++j)
                {
                    if (newProgramPrioritiesOrder[i] == newProgramPrioritiesOrder[j])
                    {
                        return new(StatusCodeExecutionResult.BadRequest, keyError: "DuplicateProgramIDs", error: $"ProgramId {newProgramPrioritiesOrder[i]} is duplicated!");
                    }
                }
            }

            return new(isSuccess: true);
        }

        #endregion

    }
}
