using ApplicantService.Core.Domain;
using Common.Models.Models;

namespace ApplicantService.Core.Application.Interfaces.Services
{
    public interface IRequestService
    {
        Task<ExecutionResult> CheckPermissionsAsync(Guid applicantId, Guid? managerId);
        Task<ExecutionResult<EducationDocumentTypeCache>> GetEducationDocumentTypeAsync(Guid documentId);
    }
}