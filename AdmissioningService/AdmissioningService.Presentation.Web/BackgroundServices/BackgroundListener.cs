using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Application.Mappers;
using AdmissioningService.Core.Domain;
using Common.Models.DTOs;
using Common.ServiceBus.ServiceBusDTOs.FromDictionaryService.Notifications;
using EasyNetQ.AutoSubscribe;

namespace AdmissioningService.Presentation.Web.BackgroundServices
{
    public class BackgroundListener : IConsumeAsync<EducationLevelAndEducationDocumentTypeAddedNotification>
    {
        private readonly IEducationLevelCacheRepository _educationLevelCacheRepository;
        private readonly IEducationDocumentTypeCacheRepository _educationDocumentTypeCacheRepository;

        public BackgroundListener(
            IEducationLevelCacheRepository educationLevelCacheRepository, 
            IEducationDocumentTypeCacheRepository educationDocumentTypeCacheRepository) 
        {
            _educationLevelCacheRepository = educationLevelCacheRepository;
            _educationDocumentTypeCacheRepository = educationDocumentTypeCacheRepository;
        }

        public async Task ConsumeAsync(EducationLevelAndEducationDocumentTypeAddedNotification message, CancellationToken cancellationToken = default)
        {
            List<EducationLevelDTO> educationLevels = message.EducationLevels;
            foreach(var educationLevel in educationLevels)
            {
                bool levelExist = await _educationLevelCacheRepository.AnyByIdAsync(educationLevel.Id);
                if (!levelExist)
                {
                    EducationLevelCache newEducationLevel = educationLevel.ToEducationLevelCache();
                    await _educationLevelCacheRepository.AddAsync(newEducationLevel);
                }
            }

            List<EducationLevelCache> educationLevelsCache = await _educationLevelCacheRepository.GetAllAsync();
            List<EducationDocumentTypeDTO> documentTypes = message.EducationDocumentTypes;
            foreach(var documentType in documentTypes) 
            {
                EducationDocumentTypeCache? documentTypeCache = await _educationDocumentTypeCacheRepository.GetByIdAsync(documentType.Id);
                if(documentTypeCache is null)
                {
                    EducationDocumentTypeCache newDocumentType = documentType.ToEducationDocumentTypeCache(educationLevelsCache);
                    await _educationDocumentTypeCacheRepository.AddAsync(newDocumentType);
                }
            }
        }
    }
}
