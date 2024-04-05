using Common.Models;

namespace ApplicantService.Core.Application.Interfaces.Repositories
{
    public interface IFileRepository
    {
        Task<ExecutionResult> DeleteAllFromDocumentAsync(Guid documentId);
    }
}
