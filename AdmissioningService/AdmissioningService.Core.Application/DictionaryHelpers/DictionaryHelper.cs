using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Application.Interfaces.Services;
using AdmissioningService.Core.Application.Mappers;
using AdmissioningService.Core.Domain;
using Common.Models.DTOs;
using Common.Models.Models;
using Common.ServiceBus.ServiceBusDTOs.FromDictionaryService.Requests;

namespace AdmissioningService.Core.DictionaryHelpers
{
    public class DictionaryHelper
    {
        private readonly IEducationDocumentTypeCacheRepository _educationDocumentTypeCacheRepository;
        private readonly IEducationLevelCacheRepository _educationLevelCacheRepository;
        private readonly IEducationProgramCacheRepository _educationProgramCacheRepository;
        private readonly IFacultyCacheRepository _facultyCacheRepository;
        private readonly IRequestService _requestService;

        public DictionaryHelper(
            IEducationDocumentTypeCacheRepository educationDocumentTypeCacheRepository, 
            IEducationLevelCacheRepository educationLevelCacheRepository,
            IEducationProgramCacheRepository educationProgramCacheRepository,
            IFacultyCacheRepository facultyCacheRepository,
            IRequestService requestService)
        {
            _educationDocumentTypeCacheRepository = educationDocumentTypeCacheRepository;
            _educationLevelCacheRepository = educationLevelCacheRepository;
            _educationProgramCacheRepository = educationProgramCacheRepository;
            _facultyCacheRepository = facultyCacheRepository;
            _requestService = requestService;
        }

        public async Task<List<EducationLevelCache>> ToEducationLevelFromDbAsync(IEnumerable<Guid> educationLevelId)
        {
            List<EducationLevelCache> EducationLevelsFromDb = new();
            foreach (var documentTypeId in educationLevelId)
            {
                var EducationLevelFromDb = await _educationLevelCacheRepository.GetByIdAsync(documentTypeId);

                EducationLevelsFromDb.Add(EducationLevelFromDb ?? throw new NullReferenceException());
            }
            return EducationLevelsFromDb;
        }

        public async Task<List<EducationDocumentTypeCache>> ToDocumentTypeFromDbAsync(IEnumerable<Guid> documentTypesId)
        {
            List<EducationDocumentTypeCache> documentTypesFromDb = new();
            foreach (var documentTypeId in documentTypesId)
            {
                var documentTypeFromDb = await _educationDocumentTypeCacheRepository.GetByIdAsync(documentTypeId);

                documentTypesFromDb.Add(documentTypeFromDb ?? throw new NullReferenceException());
            }
            return documentTypesFromDb;
        }

        public async Task<ExecutionResult> CheckProgramAsync(Guid programId)
        {
            bool programExist = await _educationProgramCacheRepository.AnyByIdAsync(programId);
            if (!programExist)
            {
                ExecutionResult<GetEducationProgramResponse> result = await _requestService.GetEducationProgramAsync(programId);
                if (!result.IsSuccess) return result;
                EducationProgramDTO program = result.Result!.EducationProgram;

                await CheckEducationLevelAsync(program.EducationLevel);
                await CheckFacultyAsync(program.Faculty);

                EducationProgramCache newProgram = new()
                {
                    Id = program.Id,
                    Code = program.Code,
                    Name = program.Name,
                    EducationForm = program.EducationForm, 
                    Language = program.Language,
                    EducationLevelId = program.EducationLevel.Id,
                    FacultyId = program.Faculty.Id,
                    Deprecated = false,
                };

                await _educationProgramCacheRepository.AddAsync(newProgram);
            }

            return new(isSuccess: true);
        }

        public async Task<ExecutionResult<EducationDocumentTypeCache>> GetEducationDocumentTypeAsync(Guid documentTypeId)
        {
            ExecutionResult<GetEducationDocumentTypeResponse> result = await _requestService.GetEducationDocumentTypeAsync(documentTypeId);
            if (!result.IsSuccess) return new() { Errors = result.Errors };
            EducationDocumentTypeDTO documentType = result.Result!.EducationDocumentType;

            await CheckNextEducationLevelsAsync(documentType.NextEducationLevel);
            await CheckEducationLevelAsync(documentType.EducationLevel);

            IEnumerable<Guid> newNextEducationLevelsId = documentType.NextEducationLevel.Select(educationLevel => educationLevel.Id);
            EducationDocumentTypeCache newDocumentType = new()
            {
                Id = documentType.Id,
                Name = documentType.Name,
                EducationLevelId = documentType.EducationLevel.Id,
                NextEducationLevel = await ToEducationLevelFromDbAsync(newNextEducationLevelsId),
                Deprecated = false,
            };

            await _educationDocumentTypeCacheRepository.AddAsync(newDocumentType);

            return new() { Result = newDocumentType };
        }

        private async Task CheckNextEducationLevelsAsync(IEnumerable<EducationLevelDTO> nextEducationLevels)
        {
            foreach (var newNextEducationLevel in nextEducationLevels)
            {
                await CheckEducationLevelAsync(newNextEducationLevel);
            }
        }

        private async Task CheckEducationLevelAsync(EducationLevelDTO educationLevel)
        {
            bool existLevel = await _educationLevelCacheRepository.AnyByIdAsync(educationLevel.Id);
            if (!existLevel)
            {
                EducationLevelCache newEducationLevel = educationLevel.ToEducationLevelCache();
                await _educationLevelCacheRepository.AddAsync(newEducationLevel);
            }
        }

        private async Task CheckFacultyAsync(FacultyDTO faculty)
        {
            bool facultyExist = await _facultyCacheRepository.AnyByIdAsync(faculty.Id);
            if (!facultyExist)
            {
                FacultyCache newFaculty = faculty.ToFacultyCache();
                await _facultyCacheRepository.AddAsync(newFaculty);
            }
        }
    }
}
