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
        private readonly IRequestService _requestService;

        public DictionaryHelper(
            IEducationDocumentTypeCacheRepository educationDocumentTypeCacheRepository, 
            IEducationLevelCacheRepository educationLevelCacheRepository,
            IRequestService requestService)
        {
            _educationDocumentTypeCacheRepository = educationDocumentTypeCacheRepository;
            _educationLevelCacheRepository = educationLevelCacheRepository;
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

        public async Task<ExecutionResult<EducationDocumentTypeCache>> GetEducationDocumentTypeAsync(Guid documentTypeId)
        {
            ExecutionResult<GetEducationDocumentTypeResponse> result = await _requestService.GetEducationDocumentTypeAsync(documentTypeId);
            if (!result.IsSuccess) return new() { Errors = result.Errors };
            EducationDocumentTypeDTO documentType = result.Result!.EducationDocumentType;

            await CheckNextEducationLevelAsync(documentType.NextEducationLevel);
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

        private async Task CheckNextEducationLevelAsync(IEnumerable<EducationLevelDTO> nextEducationLevels)
        {
            foreach (var newNextEducationLevel in nextEducationLevels)
            {
                await CheckEducationLevelAsync(newNextEducationLevel);
            }
        }

        private async Task CheckEducationLevelAsync(EducationLevelDTO educationLevels)
        {
            bool existLevel = await _educationLevelCacheRepository.AnyByIdAsync(educationLevels.Id);
            if (!existLevel)
            {
                EducationLevelCache educationLevel = educationLevels.ToEducationLevelCache();
                await _educationLevelCacheRepository.AddAsync(educationLevel);
            }
        }
    }
}
