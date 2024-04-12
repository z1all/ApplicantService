using DictionaryService.Core.Application.DTOs;
using DictionaryService.Core.Application.Interfaces.Repositories;
using DictionaryService.Core.Application.Interfaces.Services;
using DictionaryService.Core.Application.UpdateDictionaryTools.UpdateActionsCreators.Base;
using DictionaryService.Core.Domain;
using Common.Models;

namespace DictionaryService.Core.Application.UpdateDictionaryTools.UpdateActionsCreators
{
    public class UpdateEducationLevelActionsCreator : UpdateActionsCreator<EducationLevel, EducationLevelExternalDTO>
    {
        private readonly IEducationLevelRepository _educationLevelRepository;
        private readonly IEducationProgramRepository _educationProgramRepository;
        private readonly IEducationDocumentTypeRepository _educationDocumentTypeRepository;
        private readonly IExternalDictionaryService _externalDictionaryService;

        public UpdateEducationLevelActionsCreator(
            IEducationLevelRepository educationLevelRepository, IEducationProgramRepository educationProgramRepository, 
            IEducationDocumentTypeRepository educationDocumentTypeRepository, IExternalDictionaryService externalDictionaryService)
        {
            _educationLevelRepository = educationLevelRepository;
            _educationProgramRepository = educationProgramRepository;
            _educationDocumentTypeRepository = educationDocumentTypeRepository;
            _externalDictionaryService = externalDictionaryService;
        }

        protected override bool CompareKey(EducationLevel educationLevel, EducationLevelExternalDTO externalEducationLevel)
            => educationLevel.ExternalId == externalEducationLevel.Id;

        protected override async Task<List<EducationLevel>> GetEntityAsync()
            => await _educationLevelRepository.GetAllAsync();

        protected override async Task<ExecutionResult<List<EducationLevelExternalDTO>>> GetExternalEntityAsync()
            => await _externalDictionaryService.GetEducationLevelsAsync();

        protected override void UpdateEntity(EducationLevel educationLevel, EducationLevelExternalDTO externalEducationLevel)
        {
            educationLevel.Name = externalEducationLevel.Name;
            educationLevel.Deprecated = false;
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
/*
            +++CompareKey<EducationLevel, EducationLevelExternalDTO> compareKey = (educationLevel, externalEducationLevel) => educationLevel.ExternalId == externalEducationLevel.Id;
            +++GetEntityAsync<EducationLevel> getEntityAsync = _educationLevelRepository.GetAllAsync;
            +++GetExternalEntityAsync<EducationLevelExternalDTO> getExternalEntityAsync = _externalDictionaryService.GetEducationLevelsAsync;
            OnUpdateEntity<EducationLevel, EducationLevelExternalDTO> onUpdateEntity = (educationLevel, externalEducationLevel) =>
            {
                educationLevel.Name = externalEducationLevel.Name;
                educationLevel.Deprecated = false;
            };

            OnAddEntity<EducationLevel, EducationLevelExternalDTO> onAddEntity = (externalEducationLevel) =>
            {
                EducationLevel newEducationLevel = new()
                {
                    ExternalId = externalEducationLevel.Id,
                    Name = externalEducationLevel.Name,
                    Deprecated = false,
                };

                return newEducationLevel;
            };
            OnDeleteEntityAsync<EducationLevel> onDeleteEntityAsync = async (educationLevel, comments) =>
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
            };
 */