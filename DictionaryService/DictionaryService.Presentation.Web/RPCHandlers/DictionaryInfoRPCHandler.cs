using DictionaryService.Core.Application.Interfaces.Services;
using Common.ServiceBus.EasyNetQAutoSubscriber;
using EasyNetQ;
using Common.Models.Models;
using Common.ServiceBus.ServiceBusDTOs.FromDictionaryService.Requests;

namespace DictionaryService.Presentation.Web.RPCHandlers
{
    public class DictionaryInfoRPCHandler : BaseEasyNetQRPCHandler
    {
        public DictionaryInfoRPCHandler(IServiceProvider serviceProvider, IBus bus) : base(serviceProvider, bus) { }

        public override void CreateRequestListeners()
        {
            _bus.Rpc.Respond<GetFacultiesRequest, ExecutionResult<GetFacultiesResponse>>(async (_) =>
                await ExceptionHandlerAsync(GetFacultiesAsync));

            _bus.Rpc.Respond<GetEducationProgramsRequest, ExecutionResult<GetEducationProgramsResponse>>(async (request) =>
                await ExceptionHandlerAsync(async (service) => await GetProgramAsync(service, request)));

            _bus.Rpc.Respond<GetEducationLevelsRequest, ExecutionResult<GetEducationLevelsResponse>>(async (_) =>
                await ExceptionHandlerAsync(GetEducationLevelsAsync));

            _bus.Rpc.Respond<GetDocumentTypeRequest, ExecutionResult<GetDocumentTypeResponse>>(async (_) =>
                await ExceptionHandlerAsync(GetDocumentTypeAsync));

            _bus.Rpc.Respond<GetEducationDocumentTypeRequest, ExecutionResult<GetEducationDocumentTypeResponse>>(async (request) =>
                await ExceptionHandlerAsync(async (service) => await GetDocumentTypeAsync(service, request)));


            /*
                await _dictionaryInfoService.GetFacultiesAsync();
                await _dictionaryInfoService.GetProgramsAsync(request.ProgramFilter);
                await _dictionaryInfoService.GetEducationLevelsAsync();
                await _dictionaryInfoService.GetDocumentTypesAsync();
                await _dictionaryInfoService.GetDocumentTypeByIdAsync(requests.DocumentId);
            */
        }

        private async Task<ExecutionResult<GetFacultiesResponse>> GetFacultiesAsync(IServiceProvider service)
        {
            return await GetDictionaryHandlerAsync(service,
                async (_dictionaryInfoService) => await _dictionaryInfoService.GetFacultiesAsync(),
                (faculties) => new GetFacultiesResponse() { Faculties = faculties });
        }

        private async Task<ExecutionResult<GetEducationProgramsResponse>> GetProgramAsync(IServiceProvider service, GetEducationProgramsRequest request)
        {
            return await GetDictionaryHandlerAsync(service,
                async (_dictionaryInfoService) => await _dictionaryInfoService.GetProgramsAsync(request.ProgramFilter),
                (programs) => new GetEducationProgramsResponse() { ProgramPagedDTO = programs });
        }

        private async Task<ExecutionResult<GetEducationLevelsResponse>> GetEducationLevelsAsync(IServiceProvider service)
        {
            return await GetDictionaryHandlerAsync(service, 
                async (_dictionaryInfoService) => await _dictionaryInfoService.GetEducationLevelsAsync(), 
                (educationLevels) => new GetEducationLevelsResponse() { EducationLevels = educationLevels });
        }
        private async Task<ExecutionResult<GetDocumentTypeResponse>> GetDocumentTypeAsync(IServiceProvider service)
        {
            return await GetDictionaryHandlerAsync(service,
                async (_dictionaryInfoService) => await _dictionaryInfoService.GetDocumentTypesAsync(),
                (documentTypes) => new GetDocumentTypeResponse() { DocumentTypes = documentTypes });
        }

        private async Task<ExecutionResult<GetEducationDocumentTypeResponse>> GetDocumentTypeAsync(IServiceProvider service, GetEducationDocumentTypeRequest requests)
        {
            return await GetDictionaryHandlerAsync(service,
                async (_dictionaryInfoService) => await _dictionaryInfoService.GetDocumentTypeByIdAsync(requests.DocumentId),
                (documentTypes) => new GetEducationDocumentTypeResponse() 
                { 
                   Id = documentTypes.Id,
                   Name = documentTypes.Name,
                   Deprecated = false,
                });
        }


        private async Task<ExecutionResult<TResponse>> GetDictionaryHandlerAsync<TResponse, TDictionaryResponse>(
            IServiceProvider service, 
            Func<IDictionaryInfoService, Task<ExecutionResult<TDictionaryResponse>>> getOperationAsync,
            Func<TDictionaryResponse, TResponse> returnOperation)
        {
            var _dictionaryInfoService = service.GetRequiredService<IDictionaryInfoService>();

            ExecutionResult<TDictionaryResponse> result = await getOperationAsync(_dictionaryInfoService);
            if (!result.IsSuccess)
            {
                return new() { Errors = result.Errors };
            }

            return new() { Result = returnOperation(result.Result!) };
        }
    }
}
