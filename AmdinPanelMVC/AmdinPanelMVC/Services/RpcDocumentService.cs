using AmdinPanelMVC.DTOs;
using AmdinPanelMVC.Services.Base;
using AmdinPanelMVC.Services.Interfaces;
using ApplicantService.Core.Application.DTOs;
using Common.Models.DTOs.Applicant;
using Common.Models.Models;
using Common.ServiceBus.ServiceBusDTOs.FromApplicantService.Requests;
using EasyNetQ;

namespace AmdinPanelMVC.Services
{
    public class RpcDocumentService : BaseRpcService, IDocumentService
    {
        public RpcDocumentService(IBus bus) : base(bus) { }

        public async Task<ExecutionResult<PassportInfo>> GetPassportAsync(Guid applicantId)
        {
            ExecutionResult<GetPassportResponse> response
               = await RequestHandlerAsync<ExecutionResult<GetPassportResponse>, GetPassportRequest>(
                    new() { ApplicantId = applicantId }, "GetPassportFail");

            return ResponseHandler(response, passport => passport.Passport);
        }

        public async Task<ExecutionResult<List<ScanInfo>>> GetDocumentScansAsync(Guid applicantId, Guid documentId)
        {
            ExecutionResult<GetDocumentScansResponse> response
              = await RequestHandlerAsync<ExecutionResult<GetDocumentScansResponse>, GetDocumentScansRequest>(
                   new() { ApplicantId = applicantId, DocumentId = documentId }, "GetDocumentScansFail");

            return ResponseHandler(response, scans => scans.Scans);
        }

        public async Task<ExecutionResult> ChangePassportAsync(ChangePassportDTO changePassport, Guid managerId)
        {
            return await RequestHandlerAsync<ExecutionResult, ChangePassportRequest>(
                   new()
                   {
                       ApplicantId = changePassport.ApplicantId,
                       MangerId = managerId,
                       BirthPlace = changePassport.BirthPlace,
                       IssuedByWhom = changePassport.IssuedByWhom,
                       IssueYear = changePassport.IssueYear,
                       SeriesNumber = changePassport.SeriesNumber,
                   }, "ChangePassportFail");
        }

        public async Task<ExecutionResult> AddDocumentScansAsync(Guid applicantId, Guid documentId, FileDTO file, Guid managerId)
        {
            return await RequestHandlerAsync<ExecutionResult, AddDocumentScanRequest>(
                   new()
                   {
                       ApplicantId = applicantId,
                       DocumentId = documentId,
                       ManagerId = managerId,
                       File = file,
                   }, "AddDocumentScansFail");
        }

        public async Task<ExecutionResult> DeleteDocumentScansAsync(Guid applicantId, Guid documentId, Guid scanId, Guid managerId)
        {
            return await RequestHandlerAsync<ExecutionResult, DeleteDocumentScanRequest>(
                   new()
                   {
                       ApplicantId = applicantId,
                       DocumentId = documentId,
                       ScanId = scanId,
                       ManagerId = managerId,
                   }, "DeleteDocumentScansFail");
        }

        public async Task<ExecutionResult<FileDTO>> GetScanAsync(Guid applicantId, Guid documentId, Guid scanId)
        {
            ExecutionResult<GetScanResponse> response
             = await RequestHandlerAsync<ExecutionResult<GetScanResponse>, GetScanRequest>(
                  new() { ApplicantId = applicantId, DocumentId = documentId, ScanId = scanId }, "GetScanFail");

            return ResponseHandler(response, scan => scan.File);
        }

        public async Task<ExecutionResult<EducationDocumentInfo>> GetEducationDocumentAsync(Guid applicantId, Guid documentId)
        {
            ExecutionResult<GetEducationDocumentResponse> response
             = await RequestHandlerAsync<ExecutionResult<GetEducationDocumentResponse>, GetEducationDocumentRequest>(
                  new() { ApplicantId = applicantId, DocumentId = documentId }, "GetEducationDocumentFail");

            return ResponseHandler(response, document => document.EducationDocument);
        }

        public async Task<ExecutionResult> ChangeEducationDocumentAsync(ChangeEducationDocumentDTO changeEducationDocument, Guid managerId)
        {
            return await RequestHandlerAsync<ExecutionResult, ChangeEducationDocumentRequest>(
                  new()
                  {
                      ApplicantId = changeEducationDocument.ApplicantId,
                      DocumentId = changeEducationDocument.DocumentId,
                      EducationDocumentTypeId = changeEducationDocument.EducationDocumentTypeId,
                      Name = changeEducationDocument.Name,
                      ManagerId = managerId
                  }, "ChangeEducationDocumentFail");
        }
    }
}
