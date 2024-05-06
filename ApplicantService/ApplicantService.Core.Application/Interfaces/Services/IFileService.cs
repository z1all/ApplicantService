using ApplicantService.Core.Application.DTOs;
using Common.Models.Models;

namespace ApplicantService.Core.Application.Interfaces.Services
{
    public interface IFileService
    {
        Task<ExecutionResult<FileDTO>> GetApplicantScanAsync(Guid documentId, Guid scanId, Guid applicantId);
        Task<ExecutionResult> DeleteApplicantScanAsync(Guid documentId, Guid scanId, Guid applicantId, Guid? managerId = null);
        Task<ExecutionResult> AddApplicantScanAsync(Guid documentId, Guid applicantId, FileDTO file, Guid? managerId = null);
    }
}
