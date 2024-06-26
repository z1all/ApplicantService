﻿using Microsoft.Extensions.Logging;
using DictionaryService.Core.Application.DTOs;
using DictionaryService.Core.Application.UpdateDictionaryTools.UpdateActionsCreators.Base;
using DictionaryService.Core.Application.Interfaces.Repositories;
using DictionaryService.Core.Application.Interfaces.Services;
using DictionaryService.Core.Domain;
using Common.Repositories;
using Common.Models.Models;
using Common.Models.Enums;

namespace DictionaryService.Core.Application.UpdateDictionaryTools.UpdateActionsCreators
{
    public class UpdateFacultyActionsCreator : UpdateActionsCreator<Faculty, FacultyExternalDTO>
    {
        private readonly IFacultyRepository _facultyRepository;
        private readonly IEducationProgramRepository _educationProgramRepository;
        private readonly IExternalDictionaryService _externalDictionaryService;
        private readonly IUpdateStatusRepository _updateStatusRepository;
        private UpdateStatus? _updateStatusCache = null;

        private readonly ILogger<Faculty> _logger;

        protected override UpdateStatus UpdateStatusCache => _updateStatusCache!;
        protected override IUpdateStatusRepository UpdateStatusRepository => _updateStatusRepository;
        protected override IBaseWithBaseEntityRepository<Faculty> Repository => _facultyRepository;

        protected override ILogger<Faculty> Logger => _logger;

        public UpdateFacultyActionsCreator(
            ILogger<Faculty> logger,
            IFacultyRepository facultyRepository, IEducationProgramRepository educationProgramRepository, 
            IExternalDictionaryService externalDictionaryService, IUpdateStatusRepository updateStatusRepository)
        {
            _logger = logger;
            _facultyRepository = facultyRepository;
            _educationProgramRepository = educationProgramRepository;
            _externalDictionaryService = externalDictionaryService;
            _updateStatusRepository = updateStatusRepository;
        }

        protected override async Task BeforeActionsAsync()
        {
            _updateStatusCache = await _updateStatusRepository.GetByDictionaryTypeAsync(DictionaryType.Faculty);

            await base.BeforeActionsAsync();
        }

        protected override bool CompareKey(Faculty faculty, FacultyExternalDTO externalFaculty)
            => faculty.Id == externalFaculty.Id;

        protected override async Task<List<Faculty>> GetEntityAsync()
            => await _facultyRepository.GetAllAsync(true);

        protected override async Task<ExecutionResult<List<FacultyExternalDTO>>> GetExternalEntityAsync()
            => await _externalDictionaryService.GetFacultiesAsync();

        protected override bool UpdateEntity(Faculty faculty, FacultyExternalDTO externalFaculty)
        {
            if(faculty.Name != externalFaculty.Name || faculty.Deprecated != false)
            {
                faculty.Name = externalFaculty.Name;
                faculty.Deprecated = false;
                return true;
            }
            return false;
        }

        protected override Faculty AddEntity(FacultyExternalDTO externalFaculty)
        {
            Faculty newFaculty = new()
            {
                Id = externalFaculty.Id,
                Name = externalFaculty.Name,
                Deprecated = false,
            };

            return newFaculty;
        }

        protected override async Task<bool> DeleteEntityAsync(bool deleteRelatedEntities, Faculty faculty, List<string> comments)
        {
            bool thereAreRelated = false;

            List<EducationProgram> educationPrograms = await _educationProgramRepository.GetAllByFacultyIdAsync(faculty.Id);
            thereAreRelated |= SoftDeleteEntityIf(deleteRelatedEntities, educationPrograms, comments, entity =>
                $"The educational program '{entity.Name}' refers to the education level '{faculty.Name}'.");

            return thereAreRelated;
        }
    }
}