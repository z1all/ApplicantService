using ApplicantService.Core.Application.DTOs;
using ApplicantService.Core.Application.Interfaces.Services;
using Common.Models;

namespace ApplicantService.Core.Application.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly IRequestService _requestService;

        public DocumentService(IRequestService requestService)
        {
            _requestService = requestService;
        }

        public Task<ExecutionResult> DeleteApplicantDocumentAsync(Guid documentId, Guid applicantId)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult<List<DocumentInfo>>> GetApplicantDocumentsAsync(Guid applicantId)
        {
            throw new NotImplementedException();
        }       

        /////
        public Task<ExecutionResult<PassportInfo>> GetApplicantPassportAsync(Guid applicantId)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult> AddApplicantPassportAsync(EditAddPassportInfo documentInfo, Guid applicantId)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult> UpdateApplicantPassportAsync(EditAddPassportInfo documentInfo, Guid applicantId)
        {
            throw new NotImplementedException();
        }

        /////
        public Task<ExecutionResult<EducationDocumentInfo>> GetApplicantEducationDocumentAsync(Guid documentId, Guid applicantId)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult> AddApplicantEducationDocumentAsync(Guid applicantId, EditAddEducationDocumentInfo documentInfo)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult> UpdateApplicantEducationDocumentAsync(Guid documentId, Guid applicantId, EditAddEducationDocumentInfo documentInfo)
        {
            throw new NotImplementedException();
        }
    }
}