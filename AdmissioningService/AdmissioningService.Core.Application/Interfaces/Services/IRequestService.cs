﻿using Common.Models.Models;
using Common.ServiceBus.ServiceBusDTOs.FromApplicantService.Requests;
using Common.ServiceBus.ServiceBusDTOs.FromDictionaryService.Requests;

namespace AdmissioningService.Core.Application.Interfaces.Services
{
    public interface IRequestService
    {
        Task<ExecutionResult<GetApplicantResponse>> GetApplicantAsync(Guid applicantId);
        Task<ExecutionResult<GetFacultyResponse>> GetFacultyAsync(Guid facultyId);
        Task<ExecutionResult<GetEducationDocumentTypeResponse>> GetEducationDocumentTypeAsync(Guid documentTypId);
        Task<ExecutionResult<GetEducationProgramResponse>> GetEducationProgramAsync(Guid programId);
    }
}
