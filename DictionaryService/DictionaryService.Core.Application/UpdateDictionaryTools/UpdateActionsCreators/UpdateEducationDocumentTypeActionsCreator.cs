using Microsoft.Extensions.Logging;
using DictionaryService.Core.Application.DTOs;
using DictionaryService.Core.Application.UpdateDictionaryTools.UpdateActionsCreators.Base;
using DictionaryService.Core.Domain;
using DictionaryService.Core.Application.Interfaces.Repositories;
using DictionaryService.Core.Application.Interfaces.Services;
using Common.Repositories;
using Common.Models.Models;
using Common.Models.Enums;

namespace DictionaryService.Core.Application.UpdateDictionaryTools.UpdateActionsCreators
{
    internal class UpdateEducationDocumentTypeActionsCreator : UpdateActionsCreator<EducationDocumentType, EducationDocumentTypeExternalDTO>
    {
        private readonly ILogger<EducationDocumentType> _logger;

        private readonly IEducationLevelRepository _educationLevelRepository;
        private readonly IEducationDocumentTypeRepository _educationDocumentTypeRepository;
        private readonly IExternalDictionaryService _externalDictionaryService;
        private readonly IUpdateStatusRepository _updateStatusRepository;
        private List<EducationLevel>? _educationLevelsCache = null;
        private UpdateStatus? _updateStatusCache = null;

        protected override UpdateStatus UpdateStatusCache => _updateStatusCache!;
        protected override IUpdateStatusRepository UpdateStatusRepository => _updateStatusRepository;
        protected override IBaseWithBaseEntityRepository<EducationDocumentType> Repository => _educationDocumentTypeRepository;

        protected override ILogger<EducationDocumentType> Logger => _logger;

        public UpdateEducationDocumentTypeActionsCreator(
            ILogger<EducationDocumentType> logger,
            IEducationLevelRepository educationLevelRepository, IEducationDocumentTypeRepository educationDocumentTypeRepository, 
            IExternalDictionaryService externalDictionaryService, IUpdateStatusRepository updateStatusRepository)
        {
            _logger = logger;
            _educationLevelRepository = educationLevelRepository;
            _educationDocumentTypeRepository = educationDocumentTypeRepository;
            _externalDictionaryService = externalDictionaryService;
            _updateStatusRepository = updateStatusRepository;
        }

        protected override async Task BeforeActionsAsync()
        {
            _educationLevelsCache = await _educationLevelRepository.GetAllAsync(true);
            _updateStatusCache = await _updateStatusRepository.GetByDictionaryTypeAsync(DictionaryType.EducationDocumentType);

            await base.BeforeActionsAsync();
        }

        protected override bool CompareKey(EducationDocumentType documentType, EducationDocumentTypeExternalDTO externalDocumentType)
            => documentType.Id == externalDocumentType.Id;

        protected override async Task<List<EducationDocumentType>> GetEntityAsync()
            => await _educationDocumentTypeRepository.GetAllAsync(true);

        protected override async Task<ExecutionResult<List<EducationDocumentTypeExternalDTO>>> GetExternalEntityAsync()
            => await _externalDictionaryService.GetEducationDocumentTypesAsync();

        protected override bool UpdateEntity(EducationDocumentType documentType, EducationDocumentTypeExternalDTO externalDocumentType)
        {
            EducationLevel currentEducationLevel = _educationLevelsCache!.First(educationLevel => educationLevel.ExternalId == externalDocumentType.EducationLevel.Id);

            documentType.Name = externalDocumentType.Name;
            documentType.EducationLevelId = currentEducationLevel.Id;
            documentType.Deprecated = false;

            List<EducationLevel> addNextEducationLevels = new();
            foreach (var externalNextEducationLevel in externalDocumentType.NextEducationLevels)
            {
                EducationLevel educationLevel = _educationLevelsCache!.First(educationLevel => educationLevel.ExternalId == externalNextEducationLevel.Id);

                addNextEducationLevels.Add(educationLevel);
            }
            documentType.NextEducationLevels = addNextEducationLevels;

            return true;
        }

        protected override EducationDocumentType AddEntity(EducationDocumentTypeExternalDTO externalDocumentType)
        {
            EducationLevel currentEducationLevel = _educationLevelsCache!.First(educationLevel => educationLevel.ExternalId == externalDocumentType.EducationLevel.Id);

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
                EducationLevel educationLevel = _educationLevelsCache!.First(educationLevel => educationLevel.ExternalId == externalNextEducationLevel.Id);

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
            bool currentEducationLevelExist = _educationLevelsCache!.Any(educationLevel => educationLevel.ExternalId == externalDocumentType.EducationLevel.Id);
            if (!currentEducationLevelExist)
            {
                comments.Add($"The education document type '{externalDocumentType.Name}' refers to a non-existent education level '{externalDocumentType.EducationLevel.Name}'.");
            }

            bool nextEducationLevelExist = true;
            foreach (var externalNextEducationLevel in externalDocumentType.NextEducationLevels)
            {
                bool exist = _educationLevelsCache!.Any(educationLevel => educationLevel.ExternalId == externalNextEducationLevel.Id);
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