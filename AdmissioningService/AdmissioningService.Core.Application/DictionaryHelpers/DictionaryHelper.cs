using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Domain;

namespace AdmissioningService.Core.DictionaryHelpers
{
    public class DictionaryHelper
    {
        private readonly IEducationDocumentTypeCacheRepository _educationDocumentTypeCacheRepository;
        // private readonly IEducationLevelCacheRepository _educationLevelCacheRepository;

        public DictionaryHelper(
            IEducationDocumentTypeCacheRepository educationDocumentTypeCacheRepository, IEducationLevelCacheRepository educationLevelCacheRepository)
        {
            _educationDocumentTypeCacheRepository = educationDocumentTypeCacheRepository;
            // _educationLevelCacheRepository = educationLevelCacheRepository;
        }

        //[Obsolete("Если будут ошибки, то добавить взятие моделек EducationLevel из бд перед добавлением documentType")]
        //public async Task AddDocumentTypeAndEducationLevelAsync(IEnumerable<EducationDocumentTypeCache> educationDocumentTypes)
        //{
        //    foreach (var documentType in educationDocumentTypes)
        //    {
        //        bool documentTypeExist = await _educationDocumentTypeCacheRepository.AnyByIdAsync(documentType.Id);
        //        if (!documentTypeExist)
        //        {
        //            await AddEducationLevelsAsync(documentType.NextEducationLevel);
        //            await AddEducationLevelAsync(documentType.EducationLevel);    
                    
        //            await _educationDocumentTypeCacheRepository.AddAsync(documentType);
        //        }
        //    }
        //}

        //private async Task AddEducationLevelsAsync(IEnumerable<EducationLevelCache> educationLevelsCache)
        //{
        //    foreach (var level in educationLevelsCache)
        //    {
        //        await AddEducationLevelAsync(level);
        //    }
        //}

        //private async Task AddEducationLevelAsync(EducationLevelCache educationLevels)
        //{
        //    bool levelExist = await _educationLevelCacheRepository.AnyByIdAsync(educationLevels.Id);
        //    if (!levelExist)
        //    {
        //        await _educationLevelCacheRepository.AddAsync(educationLevels);
        //    }
        //}

        // [Obsolete("Если не будет ошибок, то удалить")]
        public async Task<List<EducationDocumentTypeCache>> ToDocumentTypeFromDbAsync(IEnumerable<Guid> documentTypesId)
        {
            List<EducationDocumentTypeCache> documentTypesFromDb = new();
            foreach (var documentTypeId in documentTypesId)
            {
                var documentTypeFromDb = await _educationDocumentTypeCacheRepository.GetByIdAsync(documentTypeId);

                documentTypesFromDb.Add(documentTypeFromDb!); 
            }
            return documentTypesFromDb;
        }
    }
}
