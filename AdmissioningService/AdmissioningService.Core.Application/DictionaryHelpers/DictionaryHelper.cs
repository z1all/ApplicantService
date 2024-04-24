using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Domain;

namespace AdmissioningService.Core.DictionaryHelpers
{
    public class DictionaryHelper
    {
        private readonly IEducationDocumentTypeCacheRepository _educationDocumentTypeCacheRepository;

        public DictionaryHelper(IEducationDocumentTypeCacheRepository educationDocumentTypeCacheRepository)
        {
            _educationDocumentTypeCacheRepository = educationDocumentTypeCacheRepository;
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
