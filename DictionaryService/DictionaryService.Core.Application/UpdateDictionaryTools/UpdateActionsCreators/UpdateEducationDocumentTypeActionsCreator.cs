using DictionaryService.Core.Application.DTOs;
using DictionaryService.Core.Application.UpdateDictionaryTools.UpdateActionsCreators.Base;
using DictionaryService.Core.Domain;
using DictionaryService.Core.Application.Interfaces.Repositories;
using DictionaryService.Core.Application.Interfaces.Services;
using Common.Models;

namespace DictionaryService.Core.Application.UpdateDictionaryTools.UpdateActionsCreators
{
    internal class UpdateEducationDocumentTypeActionsCreator : UpdateActionsCreator<EducationDocumentType, EducationDocumentTypeExternalDTO>
    {
        private readonly IEducationLevelRepository _educationLevelRepository;
        private readonly IEducationDocumentTypeRepository _educationDocumentTypeRepository;
        private readonly IExternalDictionaryService _externalDictionaryService;

        private List<EducationLevel>? educationLevelsCache = null;

        public UpdateEducationDocumentTypeActionsCreator(IEducationLevelRepository educationLevelRepository, IEducationDocumentTypeRepository educationDocumentTypeRepository, IExternalDictionaryService externalDictionaryService)
        {
            _educationLevelRepository = educationLevelRepository;
            _educationDocumentTypeRepository = educationDocumentTypeRepository;
            _externalDictionaryService = externalDictionaryService;
        }

        protected override async Task BeforeActionsAsync()
        {
            educationLevelsCache = await _educationLevelRepository.GetAllAsync();
        }

        protected override bool CompareKey(EducationDocumentType documentType, EducationDocumentTypeExternalDTO externalDocumentType)
            => documentType.Id == externalDocumentType.Id;

        protected override async Task<List<EducationDocumentType>> GetEntityAsync()
            => await _educationDocumentTypeRepository.GetAllAsync();

        protected override async Task<ExecutionResult<List<EducationDocumentTypeExternalDTO>>> GetExternalEntityAsync()
            => await _externalDictionaryService.GetEducationDocumentTypesAsync();

        protected override void UpdateEntity(EducationDocumentType documentType, EducationDocumentTypeExternalDTO externalDocumentType)
        {
            EducationLevel currentEducationLevel = educationLevelsCache!.First(educationLevel => educationLevel.ExternalId == externalDocumentType.EducationLevel.Id);

            documentType.Name = externalDocumentType.Name;
            documentType.EducationLevelId = currentEducationLevel.Id;
            documentType.Deprecated = false;

            List<EducationLevel> addNextEducationLevels = new();
            foreach (var externalNextEducationLevel in externalDocumentType.NextEducationLevels)
            {
                EducationLevel educationLevel = educationLevelsCache!.First(educationLevel => educationLevel.ExternalId == externalNextEducationLevel.Id);

                addNextEducationLevels.Add(educationLevel);
            }
            documentType.NextEducationLevels = addNextEducationLevels;
        }

        protected override EducationDocumentType AddEntity(EducationDocumentTypeExternalDTO externalDocumentType)
        {
            EducationLevel currentEducationLevel = educationLevelsCache!.First(educationLevel => educationLevel.ExternalId == externalDocumentType.EducationLevel.Id);

            EducationDocumentType documentType = new()
            {
                Id = externalDocumentType.Id,
                Name = externalDocumentType.Name,
                EducationLevelId = currentEducationLevel.Id,
                Deprecated = false,
            };

            List<EducationLevel> addNextEducationLevels = new();
            foreach (var externalNextEducationLevel in externalDocumentType.NextEducationLevels)
            {
                EducationLevel educationLevel = educationLevelsCache!.First(educationLevel => educationLevel.ExternalId == externalNextEducationLevel.Id);

                addNextEducationLevels.Add(educationLevel);
            }
            documentType.NextEducationLevels = addNextEducationLevels;

            return documentType;
        }

        protected override async Task<bool> CheckBeforeUpdateEntityAsync(EducationDocumentType _, EducationDocumentTypeExternalDTO externalDocumentType, List<string> comments)
        {
            return await CheckBeforeAddEntityAsync(externalDocumentType, comments);
        }

        protected override Task<bool> CheckBeforeAddEntityAsync(EducationDocumentTypeExternalDTO externalDocumentType, List<string> comments)
        {
            bool currentEducationLevelExist = educationLevelsCache!.Any(educationLevel => educationLevel.ExternalId == externalDocumentType.EducationLevel.Id);
            if (!currentEducationLevelExist)
            {
                comments.Add($"The education document type '{externalDocumentType.Name}' refers to a non-existent education level '{externalDocumentType.EducationLevel.Name}'.");
            }

            bool nextEducationLevelExist = true;
            foreach (var externalNextEducationLevel in externalDocumentType.NextEducationLevels)
            {
                bool exist = educationLevelsCache!.Any(educationLevel => educationLevel.ExternalId == externalNextEducationLevel.Id);
                if (!exist)
                {
                    comments.Add($"The education document type '{externalDocumentType.Name}' refers to a non-existent next education level '{externalNextEducationLevel.Name}'.");
                }
                nextEducationLevelExist &= exist;
            }

            return Task.FromResult(currentEducationLevelExist && nextEducationLevelExist);
        }
    }
}
/*
 
            +++ List<EducationLevel> educationLevelsCache = await _educationLevelRepository.GetAllAsync();

            +++ GetEntityAsync<EducationDocumentType> getEntityAsync = _educationDocumentTypeRepository.GetAllAsync;
            +++ GetExternalEntityAsync<EducationDocumentTypeExternalDTO> getExternalEntityAsync = _externalDictionaryService.GetEducationDocumentTypesAsync;
            +++ CompareKey<EducationDocumentType, EducationDocumentTypeExternalDTO> compareKey = (documentType, externalDocumentType) => documentType.Id == externalDocumentType.Id;
            +++ OnUpdateEntity<EducationDocumentType, EducationDocumentTypeExternalDTO> onUpdateEntity = (documentType, externalDocumentType) =>
            {
                EducationLevel currentEducationLevel = educationLevelsCache.First(educationLevel => educationLevel.ExternalId == externalDocumentType.EducationLevel.Id);

                documentType.Name = externalDocumentType.Name;
                documentType.EducationLevelId = currentEducationLevel.Id;
                documentType.Deprecated = false;

                List<EducationLevel> addNextEducationLevels = new();
                foreach (var externalNextEducationLevel in externalDocumentType.NextEducationLevels)
                {
                    EducationLevel educationLevel = educationLevelsCache.First(educationLevel => educationLevel.ExternalId == externalNextEducationLevel.Id);

                    addNextEducationLevels.Add(educationLevel);
                }
                documentType.NextEducationLevels = addNextEducationLevels;
            };
            +++ OnAddEntity<EducationDocumentType, EducationDocumentTypeExternalDTO> onAddEntity = (externalDocumentType) =>
            {
                EducationLevel currentEducationLevel = educationLevelsCache.First(educationLevel => educationLevel.ExternalId == externalDocumentType.EducationLevel.Id);

                EducationDocumentType documentType = new()
                {
                    Id = externalDocumentType.Id,
                    Name = externalDocumentType.Name,
                    EducationLevelId = currentEducationLevel.Id,
                    Deprecated = false,
                };

                List<EducationLevel> addNextEducationLevels = new();
                foreach (var externalNextEducationLevel in externalDocumentType.NextEducationLevels)
                {
                    EducationLevel educationLevel = educationLevelsCache.First(educationLevel => educationLevel.ExternalId == externalNextEducationLevel.Id);

                    addNextEducationLevels.Add(educationLevel);
                }
                documentType.NextEducationLevels = addNextEducationLevels;

                return documentType;
            };

            CheckBeforeAddEntityAsync<EducationDocumentTypeExternalDTO> checkBeforeAdd = (externalDocumentType, comments) =>
            {
                bool currentEducationLevelExist = educationLevelsCache.Any(educationLevel => educationLevel.ExternalId == externalDocumentType.EducationLevel.Id);
                if (!currentEducationLevelExist)
                {
                    comments.Add($"The education document type '{externalDocumentType.Name}' refers to a non-existent education level '{externalDocumentType.EducationLevel.Name}'.");
                }

                bool nextEducationLevelExist = true;
                foreach (var externalNextEducationLevel in externalDocumentType.NextEducationLevels)
                {
                    bool exist = educationLevelsCache.Any(educationLevel => educationLevel.ExternalId == externalNextEducationLevel.Id);
                    if(!exist)
                    {
                        comments.Add($"The education document type '{externalDocumentType.Name}' refers to a non-existent next education level '{externalNextEducationLevel.Name}'.");
                    }
                    nextEducationLevelExist &= exist;
                }

                return Task.FromResult(currentEducationLevelExist && nextEducationLevelExist);
            };
            CheckBeforeUpdateEntityAsync<EducationDocumentType, EducationDocumentTypeExternalDTO> checkBeforeUpdate = async (documentType, externalDocumentType, comments) =>
            {
                return await checkBeforeAdd(externalDocumentType, comments);
            };
 */