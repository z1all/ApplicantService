using AdmissioningService.Core.Application.Helpers;
using AdmissioningService.Core.Application.Interfaces.Repositories;
using AdmissioningService.Core.Application.Interfaces.Services;
using AdmissioningService.Core.Application.Mappers;
using AdmissioningService.Core.Domain;
using Common.Models.DTOs;

namespace AdmissioningService.Core.Application.Services
{
    public class DictionaryBackgroundService : IDictionaryBackgroundService
    {
        private readonly IEducationLevelCacheRepository _educationLevelCacheRepository;
        private readonly IEducationDocumentTypeCacheRepository _educationDocumentTypeCacheRepository;
        private readonly IEducationProgramCacheRepository _educationProgramCacheRepository;
        private readonly IFacultyCacheRepository _facultyCacheRepository;

        private readonly DictionaryHelper _dictionaryHelper;

        public DictionaryBackgroundService(
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

        public async Task AddEducationLevelAndEducationDocumentTypeAsync(List<EducationLevelDTO> educationLevels, List<EducationDocumentTypeDTO> documentTypes)
        {
            foreach (var educationLevel in educationLevels)
            {
                bool levelExist = await _educationLevelCacheRepository.AnyByIdAsync(educationLevel.Id);
                if (!levelExist)
                {
                    EducationLevelCache newEducationLevel = educationLevel.ToEducationLevelCache();
                    await _educationLevelCacheRepository.AddAsync(newEducationLevel);
                }
            }

            List<EducationLevelCache> educationLevelsCache = await _educationLevelCacheRepository.GetAllAsync();
            foreach (var documentType in documentTypes)
            {
                EducationDocumentTypeCache? documentTypeCache = await _educationDocumentTypeCacheRepository.GetByIdAsync(documentType.Id);
                if (documentTypeCache is null)
                {
                    EducationDocumentTypeCache newDocumentType = documentType.ToEducationDocumentTypeCache(educationLevelsCache);
                    await _educationDocumentTypeCacheRepository.AddAsync(newDocumentType);
                }
            }
        }

        public async Task UpdateEducationDocumentTypeAsync(EducationDocumentTypeDTO newDocumentType, bool Deprecated)
        {
            EducationDocumentTypeCache? documentType = await _educationDocumentTypeCacheRepository.GetByIdAsync(newDocumentType.Id);
            if (documentType is not null)
            {
                IEnumerable<Guid> NextEducationLevelIds = newDocumentType.NextEducationLevel.Select(educationLevel => educationLevel.Id);

                documentType.Name = newDocumentType.Name;
                documentType.Deprecated = Deprecated;
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

        public async Task UpdateEducationLevelAsync(EducationLevelDTO newEducationLevel, bool Deprecated)
        {
            EducationLevelCache? educationLevel = await _educationLevelCacheRepository.GetByIdAsync(newEducationLevel.Id);
            if (educationLevel is not null)
            {
                educationLevel.Name = newEducationLevel.Name;
                educationLevel.Deprecated = Deprecated;

                await _educationLevelCacheRepository.UpdateAsync(educationLevel);
            }
            else
            {
                educationLevel = newEducationLevel.ToEducationLevelCache();
                await _educationLevelCacheRepository.AddAsync(educationLevel);
            }
        }
        
        public async Task UpdateEducationProgramAsync(EducationProgramDTO newEducationProgram, bool Deprecated)
        {
            EducationProgramCache? educationProgram = await _educationProgramCacheRepository.GetByIdAsync(newEducationProgram.Id);
            if (educationProgram is not null)
            {
                educationProgram.Name = newEducationProgram.Name;
                educationProgram.Code = newEducationProgram.Code;
                educationProgram.Language = newEducationProgram.Language;
                educationProgram.EducationForm = newEducationProgram.EducationForm;
                educationProgram.Deprecated = Deprecated;
                educationProgram.FacultyId = newEducationProgram.Faculty.Id;
                educationProgram.EducationLevelId = newEducationProgram.EducationLevel.Id;

                await _educationProgramCacheRepository.UpdateAsync(educationProgram);
            }
        }

        public async Task UpdateFacultyAsync(FacultyDTO newFaculty, bool Deprecated)
        {
            FacultyCache? faculty = await _facultyCacheRepository.GetByIdAsync(newFaculty.Id);
            if (faculty is not null)
            {
                faculty.Name = newFaculty.Name;
                faculty.Deprecated = Deprecated;

                await _facultyCacheRepository.UpdateAsync(faculty);
            }
        }
    }
}
