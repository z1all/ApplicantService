using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Domain;

namespace AdmissioningService.Core.DictionaryHelpers
{
    public class DictionaryHelper
    {
        private readonly IEducationDocumentTypeCacheRepository _educationDocumentTypeCacheRepository;
        private readonly IEducationLevelCacheRepository _educationLevelCacheRepository;

        public DictionaryHelper(
            IEducationDocumentTypeCacheRepository educationDocumentTypeCacheRepository, 
            IEducationLevelCacheRepository educationLevelCacheRepository)
        {
            _educationDocumentTypeCacheRepository = educationDocumentTypeCacheRepository;
            _educationLevelCacheRepository = educationLevelCacheRepository;
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
    }
}
