using Common.Models;

namespace ApplicantService.Core.Application.Interfaces.Services
{
    public interface IFileService
    {
        Task<ExecutionResult> GetApplicantScan(Guid documentId, Guid scanId, Guid applicantId);
        Task<ExecutionResult> DeleteApplicantScan(Guid documentId, Guid scanId, Guid applicantId);
        Task<ExecutionResult> AddApplicantScan(Guid documentId, Guid scanId, Guid applicantId);
    }
}
