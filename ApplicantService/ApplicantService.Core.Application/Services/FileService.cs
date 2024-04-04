using ApplicantService.Core.Application.Interfaces.Services;
using Common.Models;

namespace ApplicantService.Core.Application.Services
{
    public class FileService : IFileService
    {
        private readonly IRequestService _requestService;

        public FileService(IRequestService requestService)
        {
            _requestService = requestService;
        }

        public Task<ExecutionResult> GetApplicantScanAsync(Guid documentId, Guid scanId, Guid applicantId)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult> AddApplicantScanAsync(Guid documentId, Guid scanId, Guid applicantId)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult> DeleteApplicantScanAsync(Guid documentId, Guid scanId, Guid applicantId)
        {
            throw new NotImplementedException();
        }
    }
}
