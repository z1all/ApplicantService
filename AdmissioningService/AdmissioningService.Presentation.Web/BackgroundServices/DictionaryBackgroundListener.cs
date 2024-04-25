using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Application.Mappers;
using AdmissioningService.Core.DictionaryHelpers;
using AdmissioningService.Core.Domain;
using Common.Models.DTOs;
using Common.ServiceBus.ServiceBusDTOs.FromDictionaryService.Notifications;
using Common.ServiceBusDTOs.FromUserService;
using EasyNetQ.AutoSubscribe;

namespace AdmissioningService.Presentation.Web.BackgroundServices
{
    public class DictionaryBackgroundListener : 
        IConsumeAsync<EducationLevelAndEducationDocumentTypeAddedNotification>,
        IConsumeAsync<EducationDocumentTypeUpdatedNotification>,
        IConsumeAsync<EducationLevelUpdatedNotification>,
        IConsumeAsync<EducationProgramUpdatedNotification>,
        IConsumeAsync<FacultyUpdatedNotification>
    {
        private readonly IEducationLevelCacheRepository _educationLevelCacheRepository;
        private readonly IEducationDocumentTypeCacheRepository _educationDocumentTypeCacheRepository;
        private readonly IEducationProgramCacheRepository _educationProgramCacheRepository;
        private readonly IFacultyCacheRepository _facultyCacheRepository;
        
        private readonly DictionaryHelper _dictionaryHelper;

        public DictionaryBackgroundListener(
            IEducationLevelCacheRepository educationLevelCacheRepository, 
            IEducationDocumentTypeCacheRepository educationDocumentTypeCacheRepository,
            IEducationProgramCacheRepository educationProgramCacheRepository,
            IFacultyCacheRepository facultyCacheRepository,
            DictionaryHelper dictionaryHelper) 
        {
            _educationLevelCacheRepository = educationLevelCacheRepository;
            _educationDocumentTypeCacheRepository = educationDocumentTypeCacheRepository;
            _educationProgramCacheRepository = educationProgramCacheRepository;
            _facultyCacheRepository = facultyCacheRepository;

            _dictionaryHelper = dictionaryHelper;
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

        public async Task ConsumeAsync(EducationDocumentTypeUpdatedNotification message, CancellationToken cancellationToken = default)
        {
            EducationDocumentTypeDTO newDocumentType = message.EducationDocumentType;

            EducationDocumentTypeCache? documentType = await _educationDocumentTypeCacheRepository.GetByIdAsync(message.EducationDocumentType.Id);
            if (documentType is not null)
            {
                IEnumerable<Guid> NextEducationLevelIds = newDocumentType.NextEducationLevel.Select(educationLevel => educationLevel.Id);

                documentType.Name = newDocumentType.Name;
                documentType.Deprecated = message.Deprecated;
                documentType.EducationLevelId = newDocumentType.EducationLevel.Id;
                documentType.NextEducationLevel = await _dictionaryHelper.ToEducationLevelFromDbAsync(NextEducationLevelIds);

                await _educationDocumentTypeCacheRepository.UpdateAsync(documentType);
            }
            else
            {
                List<EducationLevelCache> educationLevelsCache = await _educationLevelCacheRepository.GetAllAsync();

                documentType = newDocumentType.ToEducationDocumentTypeCache(educationLevelsCache);
                await _educationDocumentTypeCacheRepository.AddAsync(documentType);
            }
        }

        public async Task ConsumeAsync(EducationLevelUpdatedNotification message, CancellationToken cancellationToken = default)
        {
            EducationLevelDTO newEducationLevel = message.EducationLevel;

            EducationLevelCache? educationLevel = await _educationLevelCacheRepository.GetByIdAsync(newEducationLevel.Id);
            if(educationLevel is not null)
            {
                educationLevel.Name = newEducationLevel.Name;
                educationLevel.Deprecated = message.Deprecated;

                await _educationLevelCacheRepository.UpdateAsync(educationLevel);
            }
            else
            {
                educationLevel = newEducationLevel.ToEducationLevelCache();
                await _educationLevelCacheRepository.AddAsync(educationLevel);
            }
        }

        public async Task ConsumeAsync(EducationProgramUpdatedNotification message, CancellationToken cancellationToken = default)
        {
            EducationProgramDTO newEducationProgram = message.EducationProgram;

            EducationProgramCache? educationProgram = await _educationProgramCacheRepository.GetByIdAsync(newEducationProgram.Id);
            if(educationProgram is not null)
            {
                educationProgram.Name = newEducationProgram.Name;
                educationProgram.Code = newEducationProgram.Code;
                educationProgram.Language = newEducationProgram.Language;
                educationProgram.EducationForm = newEducationProgram.EducationForm;
                educationProgram.Deprecated = message.Deprecated;
                educationProgram.FacultyId = newEducationProgram.Faculty.Id;
                educationProgram.EducationLevelId = newEducationProgram.EducationLevel.Id;

                await _educationProgramCacheRepository.UpdateAsync(educationProgram);
            }
        }

        public async Task ConsumeAsync(FacultyUpdatedNotification message, CancellationToken cancellationToken = default)
        {
            FacultyDTO newFaculty = message.Faculty;

            FacultyCache? faculty = await _facultyCacheRepository.GetByIdAsync(newFaculty.Id);
            if(faculty is not null)
            {
                faculty.Name = newFaculty.Name;
                faculty.Deprecated = message.Deprecated;

                await _facultyCacheRepository.UpdateAsync(faculty);
            }
        }
    }
}
