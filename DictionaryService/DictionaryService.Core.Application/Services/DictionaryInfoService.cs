﻿using Microsoft.Extensions.Logging;
using DictionaryService.Core.Application.Interfaces.Services;
using DictionaryService.Core.Application.Interfaces.Repositories;
using DictionaryService.Core.Domain;
using DictionaryService.Core.Application.Mappers;
using Common.Models.Models;
using Common.Models.DTOs.Dictionary;

namespace DictionaryService.Core.Application.Services
{
    public class DictionaryInfoService : IDictionaryInfoService
    {
        private readonly ILogger<DictionaryInfoService> _logger;
        private readonly IEducationProgramRepository _educationProgramRepository;
        private readonly IEducationLevelRepository _educationLevelRepository;
        private readonly IEducationDocumentTypeRepository _educationDocumentTypeRepository;
        private readonly IFacultyRepository _facultyRepository;

        public DictionaryInfoService(
            ILogger<DictionaryInfoService> logger,
            IEducationProgramRepository educationProgramRepository, IEducationLevelRepository educationLevelRepository,
            IEducationDocumentTypeRepository educationDocumentTypeRepository, IFacultyRepository facultyRepository
        ) 
        { 
            _logger = logger;
            _educationProgramRepository = educationProgramRepository;
            _educationLevelRepository = educationLevelRepository;
            _educationDocumentTypeRepository = educationDocumentTypeRepository;
            _facultyRepository = facultyRepository;
        }

        public async Task<ExecutionResult<ProgramPagedDTO>> GetProgramsAsync(EducationProgramFilterDTO filter)
        {
            if (filter.Page < 1)
            {
                _logger.LogTrace($"Requesting programs with invalid pagination parameters. Filter page {filter.Page} < 1");
                return new(StatusCodeExecutionResult.BadRequest, keyError: "InvalidPageError", error: "Number of page can't be less than 1.");
            }

            int countPrograms = await _educationProgramRepository.GetAllCountAsync(filter, getDeprecated: false);
            countPrograms = countPrograms == 0 ? 1 : countPrograms;
            
            int countPage = (countPrograms / filter.Size) + (countPrograms % filter.Size == 0 ? 0 : 1);
            if (filter.Page > countPage) 
            {
                _logger.LogTrace($"Requesting programs with invalid pagination parameters. Filter page {filter.Page} > count page {countPage}");
                return new(StatusCodeExecutionResult.BadRequest, keyError: "InvalidPageError", error: $"Number of page can be from 1 to {countPage}.");
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
        }

        public async Task<ExecutionResult<List<EducationLevelDTO>>> GetEducationLevelsAsync()
        {
            List<EducationLevel> educationLevels = await _educationLevelRepository.GetAllAsync(false);

            return new()
            {
                Result = educationLevels.Select(educationLevel => educationLevel.ToEducationLevelDTO()).ToList(),
            };
        }

        public async Task<ExecutionResult<List<EducationDocumentTypeDTO>>> GetDocumentTypesAsync()
        {
            List<EducationDocumentType> documentTypes = await _educationDocumentTypeRepository.GetAllAsync(false);

            return new()
            {
                Result = documentTypes.Select(documentType => documentType.ToEducationDocumentTypeDTO()).ToList(),
            };
        }

        public async Task<ExecutionResult<List<FacultyDTO>>> GetFacultiesAsync()
        {
            List<Faculty> faculties = await _facultyRepository.GetAllAsync(false);

            return new()
            {
                Result = faculties.Select(faculty => faculty.ToFacultyDTO()).ToList(),
            };
        }

        public async Task<ExecutionResult<EducationDocumentTypeDTO>> GetDocumentTypeByIdAsync(Guid documentTypeId)
        {
            EducationDocumentType? documentType = await _educationDocumentTypeRepository.GetByIdAsync(documentTypeId, false);
            if(documentType is null)
            {
                _logger.LogTrace($"Request for a non-existent document type with id {documentTypeId}");
                return new(StatusCodeExecutionResult.NotFound, keyError: "DocumentTypeNotFoundError", error: $"Document type with id {documentTypeId} not found!");
            }

            return new(result: documentType.ToEducationDocumentTypeDTO());
        }

        public async Task<ExecutionResult<FacultyDTO>> GetFacultyAsync(Guid facultyId)
        {
            Faculty? faculty =  await _facultyRepository.GetByIdAsync(facultyId);
            if(faculty is null)
            {
                _logger.LogTrace($"Request for a non-existent faculty with id {facultyId}");
                return new(StatusCodeExecutionResult.NotFound, keyError: "FacultyNotFound", error: $"Faculty with id {facultyId} not found!");
            }

            return new(result: faculty.ToFacultyDTO());
        }

        public async Task<ExecutionResult<EducationProgramDTO>> GetEducationProgramByIdAsync(Guid programId)
        {
            EducationProgram? program = await _educationProgramRepository.GetByIdWithFacultyAndLevelAsync(programId);
            if (program is null)
            {
                _logger.LogTrace($"Request for a non-existent program with id {programId}");
                return new(StatusCodeExecutionResult.NotFound, keyError: "ProgramNotFound", error: $"Education program with id {programId} not found!");
            }

            return new(result: program.ToEducationProgramDTO());
        }
    }
}
