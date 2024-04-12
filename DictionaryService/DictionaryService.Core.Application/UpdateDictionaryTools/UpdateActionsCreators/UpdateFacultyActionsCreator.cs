using DictionaryService.Core.Application.DTOs;
using DictionaryService.Core.Application.UpdateDictionaryTools.UpdateActionsCreators.Base;
using DictionaryService.Core.Application.Interfaces.Repositories;
using DictionaryService.Core.Application.Interfaces.Services;
using DictionaryService.Core.Domain;
using Common.Models;

namespace DictionaryService.Core.Application.UpdateDictionaryTools.UpdateActionsCreators
{
    public class UpdateFacultyActionsCreator : UpdateActionsCreator<Faculty, FacultyExternalDTO>
    {
        private readonly IFacultyRepository _facultyRepository;
        private readonly IEducationProgramRepository _educationProgramRepository;
        private readonly IExternalDictionaryService _externalDictionaryService;

        public UpdateFacultyActionsCreator(IFacultyRepository facultyRepository, IEducationProgramRepository educationProgramRepository, IExternalDictionaryService externalDictionaryService) 
        {
            _facultyRepository = facultyRepository;
            _educationProgramRepository = educationProgramRepository;
            _externalDictionaryService = externalDictionaryService;
        }

        protected override bool CompareKey(Faculty faculty, FacultyExternalDTO externalFaculty)
            => faculty.Id == externalFaculty.Id;

        protected override async Task<List<Faculty>> GetEntityAsync()
            => await _facultyRepository.GetAllAsync();

        protected override async Task<ExecutionResult<List<FacultyExternalDTO>>> GetExternalEntityAsync()
            => await _externalDictionaryService.GetFacultiesAsync();

        protected override void UpdateEntity(Faculty faculty, FacultyExternalDTO externalFaculty)
        {
            faculty.Name = externalFaculty.Name;
            faculty.Deprecated = false;
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
/*
            +++ CompareKey<Faculty, FacultyExternalDTO> compareKey = (faculty, externalFaculty) => faculty.Id == externalFaculty.Id;
            +++GetEntityAsync<Faculty> getEntityAsync = _facultyRepository.GetAllAsync;
            +++GetExternalEntityAsync<FacultyExternalDTO> getExternalEntityAsync = _externalDictionaryService.GetFacultiesAsync;
            +++OnUpdateEntity<Faculty, FacultyExternalDTO> onUpdateEntity = (faculty, externalFaculty) =>
            {
                faculty.Name = externalFaculty.Name;
                faculty.Deprecated = false;
            };
            OnAddEntity<Faculty, FacultyExternalDTO> onAddEntity = (externalFaculty) =>
            {
                Faculty newFaculty = new()
                {
                    Id = externalFaculty.Id,
                    Name = externalFaculty.Name,
                    Deprecated = false,
                };

                return newFaculty;
            };

            OnDeleteEntityAsync<Faculty> onDeleteEntityAsync = async (faculty, comments) =>
            {
                bool thereAreRelated = false;

                List<EducationProgram> educationPrograms = await _educationProgramRepository.GetAllByFacultyIdAsync(faculty.Id);
                thereAreRelated |= SoftDeleteEntityIf(deleteRelatedEntities, educationPrograms, comments, entity =>
                    $"The educational program '{entity.Name}' refers to the education level '{faculty.Name}'.");

                return thereAreRelated;
            };
 */