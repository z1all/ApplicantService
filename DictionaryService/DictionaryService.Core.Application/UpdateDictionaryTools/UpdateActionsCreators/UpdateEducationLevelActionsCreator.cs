using DictionaryService.Core.Application.DTOs;
using DictionaryService.Core.Application.Interfaces.Repositories;
using DictionaryService.Core.Application.Interfaces.Services;
using DictionaryService.Core.Application.UpdateDictionaryTools.UpdateActionsCreators.Base;
using DictionaryService.Core.Domain;
using Common.Enums;
using Common.Models;
using Common.Repositories;

namespace DictionaryService.Core.Application.UpdateDictionaryTools.UpdateActionsCreators
{
    public class UpdateEducationLevelActionsCreator : UpdateActionsCreator<EducationLevel, EducationLevelExternalDTO>
    {
        private readonly IEducationLevelRepository _educationLevelRepository;
        private readonly IEducationProgramRepository _educationProgramRepository;
        private readonly IEducationDocumentTypeRepository _educationDocumentTypeRepository;
        private readonly IExternalDictionaryService _externalDictionaryService;
        private readonly IUpdateStatusRepository _updateStatusRepository;
        private UpdateStatus? _updateStatusCache = null;

        protected override UpdateStatus UpdateStatusCache => _updateStatusCache!;
        protected override IUpdateStatusRepository UpdateStatusRepository => _updateStatusRepository;
        protected override IBaseRepository<EducationLevel> Repository => _educationLevelRepository;

        public UpdateEducationLevelActionsCreator(
            IEducationLevelRepository educationLevelRepository, IEducationProgramRepository educationProgramRepository, 
            IEducationDocumentTypeRepository educationDocumentTypeRepository, IExternalDictionaryService externalDictionaryService,
            IUpdateStatusRepository updateStatusRepository)
        {
            _educationLevelRepository = educationLevelRepository;
            _educationProgramRepository = educationProgramRepository;
            _educationDocumentTypeRepository = educationDocumentTypeRepository;
            _externalDictionaryService = externalDictionaryService;
            _updateStatusRepository = updateStatusRepository;
        }

        protected override async Task BeforeActionsAsync()
        {
            _updateStatusCache = await _updateStatusRepository.GetByDictionaryTypeAsync(DictionaryType.EducationLevel);

            await base.BeforeActionsAsync();
        }

        protected override bool CompareKey(EducationLevel educationLevel, EducationLevelExternalDTO externalEducationLevel)
            => educationLevel.ExternalId == externalEducationLevel.Id;

        protected override async Task<List<EducationLevel>> GetEntityAsync()
            => await _educationLevelRepository.GetAllAsync();

        protected override async Task<ExecutionResult<List<EducationLevelExternalDTO>>> GetExternalEntityAsync()
            => await _externalDictionaryService.GetEducationLevelsAsync();

        protected override bool UpdateEntity(EducationLevel educationLevel, EducationLevelExternalDTO externalEducationLevel)
        {
            if (educationLevel.Name != externalEducationLevel.Name || educationLevel.Deprecated != false)
            {
                educationLevel.Name = externalEducationLevel.Name;
                educationLevel.Deprecated = false;
                return true;
            }
            return false;
        }

        protected override EducationLevel AddEntity(EducationLevelExternalDTO externalEducationLevel)
        {
            EducationLevel newEducationLevel = new()
            {
                ExternalId = externalEducationLevel.Id,
                Name = externalEducationLevel.Name,
                Deprecated = false,
            };

            return newEducationLevel;
        }

        protected override async Task<bool> DeleteEntityAsync(bool deleteRelatedEntities, EducationLevel educationLevel, List<string> comments)
        {
            bool thereAreRelated = false;

            List<EducationProgram> educationPrograms = await _educationProgramRepository.GetAllByEducationLevelIdAsync(educationLevel.Id);
            thereAreRelated |= SoftDeleteEntityIf(deleteRelatedEntities, educationPrograms, comments, entity =>
                $"The educational program '{entity.Name}' refers to the education level '{educationLevel.Name}'.");

            List<EducationDocumentType> documentTypesRelatedWithNextLevel = await _educationDocumentTypeRepository.GetAllByNextEducationLevelIdAsync(educationLevel.Id);
            thereAreRelated |= SoftDeleteEntityIf(deleteRelatedEntities, documentTypesRelatedWithNextLevel, comments, entity =>
                $"The educational document type '{entity.Name}' refers to the education level '{educationLevel.Name}'.");

            List<EducationDocumentType> documentTypesRelatedWithCurrentLevel = await _educationDocumentTypeRepository.GetByCurrentEducationLevelIdAsync(educationLevel.Id);
            thereAreRelated |= SoftDeleteEntityIf(deleteRelatedEntities, documentTypesRelatedWithCurrentLevel, comments, entity =>
                $"The educational document type '{entity.Name}' refers to the education level '{educationLevel.Name}'.");

            return thereAreRelated;
        }
    }
}