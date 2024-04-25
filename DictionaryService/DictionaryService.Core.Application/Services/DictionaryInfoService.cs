using DictionaryService.Core.Application.Interfaces.Services;
using DictionaryService.Core.Application.Interfaces.Repositories;
using DictionaryService.Core.Domain;
using DictionaryService.Core.Application.Mappers;
using Common.Models.DTOs;
using Common.Models.Models;

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

            /*
               +++ await _dictionaryInfoService.GetFacultiesAsync(); +++
               +++ await _dictionaryInfoService.GetProgramsAsync(request.ProgramFilter); +++
               +++ await _dictionaryInfoService.GetEducationLevelsAsync(); +++
               +++ await _dictionaryInfoService.GetDocumentTypesAsync(); +++
               ??? await _dictionaryInfoService.GetDocumentTypeByIdAsync(requests.DocumentId); ???
           */
        }

        public async Task<ExecutionResult<ProgramPagedDTO>> GetProgramsAsync(EducationProgramFilterDTO filter)
        {
            if (filter.Page < 1)
            {
                return new(keyError: "InvalidPageError", error: "Number of page can't be less than 1.");
            }

            int countPrograms = await _educationProgramRepository.GetAllCountAsync(filter, getDeprecated: false);
            countPrograms = countPrograms == 0 ? 1 : countPrograms;
            
            int countPage = (countPrograms / filter.Size) + (countPrograms % filter.Size == 0 ? 0 : 1);
            if (filter.Page > countPage) 
            {
                return new(keyError: "InvalidPageError", error: $"Number of page can be from 1 to {countPage}.");
            }

            List<EducationProgram> educationPrograms = await _educationProgramRepository.GetAllByFiltersAsync(filter, getDeprecated: false);
            return new()
            {
                Result = new()
                {
                    Programs = educationPrograms.Select(educationProgram => educationProgram.ToEducationProgramDTO()).ToList(),
                    Pagination = new()
                    {
                        Count = countPage,
                        Current = filter.Page,
                        Size = filter.Size,
                    },
                },
            };

            // +++ await _educationProgramRepository.GetAllCountAsync(filter);
            // +++ await _educationProgramRepository.GetAllByFiltersAsync(filter);
        }

        public async Task<ExecutionResult<List<EducationLevelDTO>>> GetEducationLevelsAsync()
        {
            List<EducationLevel> educationLevels = await _educationLevelRepository.GetAllAsync(false);

            return new()
            {
                Result = educationLevels.Select(educationLevel => educationLevel.ToEducationLevelDTO()).ToList(),
            };

            // +++ await _educationLevelRepository.GetAllAsync();
        }

        public async Task<ExecutionResult<List<EducationDocumentTypeDTO>>> GetDocumentTypesAsync()
        {
            List<EducationDocumentType> documentTypes = await _educationDocumentTypeRepository.GetAllAsync(false);

            return new()
            {
                Result = documentTypes.Select(documentType => documentType.ToEducationDocumentTypeDTO()).ToList(),
            };

            // +++ await _educationDocumentTypeRepository.GetAllAsync()
        }

        public async Task<ExecutionResult<List<FacultyDTO>>> GetFacultiesAsync()
        {
            List<Faculty> faculties = await _facultyRepository.GetAllAsync(false);

            return new()
            {
                Result = faculties.Select(faculty => faculty.ToFacultyDTO()).ToList(),
            };

            // +++ await _facultyRepository.GetAllAsync();
        }

        public async Task<ExecutionResult<EducationDocumentTypeDTO>> GetDocumentTypeByIdAsync(Guid documentTypeId)
        {
            EducationDocumentType? documentType = await _educationDocumentTypeRepository.GetByIdAsync(documentTypeId, false);
            if(documentType is null)
            {
                return new(keyError: "DocumentTypeNotFoundError", error: $"Document type with id {documentTypeId} not found!");
            }

            return new() {  Result = documentType.ToEducationDocumentTypeDTO() };

            // +++ await _educationDocumentTypeRepository.GetByIdAsync(documentTypeId);
        }

        public async Task<ExecutionResult<FacultyDTO>> GetFacultyAsync(Guid facultyId)
        {
            Faculty? faculty =  await _facultyRepository.GetByIdAsync(facultyId);
            if(faculty is null)
            {
                return new(keyError: "FacultyNotFound", error: $"Faculty with id {facultyId} not found!");
            }

            return new() { Result = faculty.ToFacultyDTO() };
        }
    }
}
