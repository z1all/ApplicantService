using DictionaryService.Core.Application.DTOs;
using DictionaryService.Core.Application.Interfaces.Services;
using DictionaryService.Core.Application.Interfaces.Repositories;
using DictionaryService.Core.Domain;
using DictionaryService.Core.Application.Mappers;
using Common.Models;

namespace DictionaryService.Core.Application.Services
{
    public class DictionaryInfoService : IDictionaryInfoService
    {
        private readonly IEducationProgramRepository _educationProgramRepository;
        private readonly IEducationLevelRepository _educationLevelRepository;
        private readonly IEducationDocumentTypeRepository _educationDocumentTypeRepository;
        private readonly IFacultyRepository _facultyRepository;

        public DictionaryInfoService(
            IEducationProgramRepository educationProgramRepository, IEducationLevelRepository educationLevelRepository,
            IEducationDocumentTypeRepository educationDocumentTypeRepository, IFacultyRepository facultyRepository
        ) 
        { 
            _educationProgramRepository = educationProgramRepository;
            _educationLevelRepository = educationLevelRepository;
            _educationDocumentTypeRepository = educationDocumentTypeRepository;
            _facultyRepository = facultyRepository;
        }

        public async Task<ExecutionResult<ProgramPagedDTO>> GetProgramsAsync(EducationProgramFilterDTO filter)
        {
            List<EducationProgram> educationPrograms = await _educationProgramRepository.GetAllByFiltersAsync(filter);

            return new()
            {
                Result = new()
                {
                    Programs = educationPrograms.Select(educationProgram => educationProgram.ToEducationProgramDTO()).ToList(),
                    Pagination = new()
                    {
                        Count = 1,
                        Current = 1,
                        Size = 1,
                    },
                },
            };
        }

        public async Task<ExecutionResult<List<EducationLevelDTO>>> GetEducationLevelsAsync()
        {
            List<EducationLevel> educationLevels = await _educationLevelRepository.GetAllAsync();

            return new()
            {
                Result = educationLevels.Select(educationLevel => educationLevel.ToEducationLevelDTO()).ToList(),
            };
        }

        public async Task<ExecutionResult<List<EducationDocumentTypeDTO>>> GetDocumentTypesAsync()
        {
            List<EducationDocumentType> documentTypes = await _educationDocumentTypeRepository.GetAllAsync();

            return new()
            {
                Result = documentTypes.Select(documentType => documentType.ToEducationDocumentTypeDTO()).ToList(),
            };
        }

        public async Task<ExecutionResult<List<FacultyDTO>>> GetFacultiesAsync()
        {
            List<Faculty> faculties = await _facultyRepository.GetAllAsync();

            return new()
            {
                Result = faculties.Select(faculty => faculty.ToFacultyDTO()).ToList(),
            };
        }
    }
}
