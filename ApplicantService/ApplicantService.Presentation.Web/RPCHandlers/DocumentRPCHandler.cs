using ApplicantService.Core.Application.Interfaces.Services;
using Common.Models.DTOs.Applicant;
using Common.Models.Models;
using Common.ServiceBus.EasyNetQRPC;
using Common.ServiceBus.ServiceBusDTOs.FromApplicantService.Requests;
using EasyNetQ;

namespace ApplicantService.Presentation.Web.RPCHandlers
{
    public class DocumentRPCHandler : BaseEasyNetQRPCHandler
    {
        public DocumentRPCHandler(IServiceProvider serviceProvider, IBus bus) : base(serviceProvider, bus) { }

        public override void CreateRequestListeners()
        {
            _bus.Rpc.Respond<GetPassportRequest, ExecutionResult<GetPassportResponse>>(async (request) =>
                await ExceptionHandlerAsync(async (service) => await GetPassportAsync(service, request)));

            _bus.Rpc.Respond<ChangePassportRequest, ExecutionResult>(async (request) =>
                await ExceptionHandlerAsync(async (service) => await ChangePassportAsync(service, request)));

            _bus.Rpc.Respond<GetDocumentScansRequest, ExecutionResult<GetDocumentScansResponse>>(async (request) =>
                await ExceptionHandlerAsync(async (service) => await GetDocumentScansAsync(service, request)));

            _bus.Rpc.Respond<AddDocumentScanRequest, ExecutionResult>(async (request) =>
               await ExceptionHandlerAsync(async (service) => await AddDocumentScansAsync(service, request)));

            _bus.Rpc.Respond<DeleteDocumentScanRequest, ExecutionResult>(async (request) =>
               await ExceptionHandlerAsync(async (service) => await DeleteDocumentScanAsync(service, request)));
        }

        private async Task<ExecutionResult<GetPassportResponse>> GetPassportAsync(IServiceProvider service, GetPassportRequest request)
        {
            var _documentService = service.GetRequiredService<IDocumentService>();

            ExecutionResult<PassportInfo> result = await _documentService.GetApplicantPassportAsync(request.ApplicantId);

            return ResponseHandler(result, passport => new GetPassportResponse() { Passport = passport });
        }

        private async Task<ExecutionResult> ChangePassportAsync(IServiceProvider service, ChangePassportRequest request)
        {
            var _documentService = service.GetRequiredService<IDocumentService>();

            return await _documentService.UpdateApplicantPassportAsync(new()
            {
                BirthPlace = request.BirthPlace,
                IssuedByWhom = request.IssuedByWhom,
                IssueYear = request.IssueYear,
                SeriesNumber = request.SeriesNumber,
            }, request.ApplicantId, request.MangerId);
        }

        private async Task<ExecutionResult<GetDocumentScansResponse>> GetDocumentScansAsync(IServiceProvider service, GetDocumentScansRequest request)
        {
            var _documentService = service.GetRequiredService<IDocumentService>();

            ExecutionResult<List<ScanInfo>> result = await _documentService.GetScansInfoAsync(request.ApplicantId, request.DocumentId);

            return ResponseHandler(result, scans => new GetDocumentScansResponse() { Scans = scans });
        }

        private async Task<ExecutionResult> AddDocumentScansAsync(IServiceProvider service, AddDocumentScanRequest request)
        {
            var _fileService = service.GetRequiredService<IFileService>();

            return await _fileService.AddApplicantScanAsync(request.DocumentId, request.ApplicantId, request.File, request.ManagerId);
        }

        private async Task<ExecutionResult> DeleteDocumentScanAsync(IServiceProvider service, DeleteDocumentScanRequest request)
        {
            var _fileService = service.GetRequiredService<IFileService>();

            return await _fileService.DeleteApplicantScanAsync(request.DocumentId, request.ScanId, request.ApplicantId, request.ManagerId);
        }
    }
}
