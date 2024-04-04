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

        public Task<ExecutionResult> DeleteApplicantDocument(Guid documentId, Guid applicantId)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult<List<DocumentInfo>>> GetApplicantDocuments(Guid applicantId)
        {
            throw new NotImplementedException();
        }       

        /////
        public Task<ExecutionResult<PassportInfo>> GetApplicantPassport(Guid applicantId)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult> AddApplicantPassport(EditAddPassportInfo documentInfo, Guid applicantId)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult> UpdateApplicantPassport(EditAddPassportInfo documentInfo, Guid applicantId)
        {
            throw new NotImplementedException();
        }

        /////
        public Task<ExecutionResult<EducationDocumentInfo>> GetApplicantEducationDocument(Guid documentId, Guid applicantId)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult> AddApplicantEducationDocument(Guid applicantId, EditAddEducationDocumentInfo documentInfo)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult> UpdateApplicantEducationDocument(Guid documentId, Guid applicantId, EditAddEducationDocumentInfo documentInfo)
        {
            throw new NotImplementedException();
        }
    }
}