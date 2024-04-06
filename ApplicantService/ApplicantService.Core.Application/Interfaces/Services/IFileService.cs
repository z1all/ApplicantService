using ApplicantService.Core.Application.DTOs;
using Common.Models;

namespace ApplicantService.Core.Application.Interfaces.Services
{
    public interface IFileService
    {
        Task<ExecutionResult> GetApplicantScanAsync(Guid documentId, Guid scanId, Guid applicantId);
        Task<ExecutionResult> DeleteApplicantScanAsync(Guid documentId, Guid scanId, Guid applicantId);
        Task<ExecutionResult> AddApplicantScanAsync(Guid documentId, Guid applicantId, FileDTO file);
    }
}
