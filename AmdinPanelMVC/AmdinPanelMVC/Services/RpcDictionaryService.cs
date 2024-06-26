﻿using AmdinPanelMVC.Services.Base;
using AmdinPanelMVC.Services.Interfaces;
using Common.Models.DTOs.Dictionary;
using Common.Models.Models;
using Common.ServiceBus.ServiceBusDTOs.FromDictionaryService.Requests;
using EasyNetQ;

namespace AmdinPanelMVC.Services
{
    public class RpcDictionaryService : BaseRpcService, IDictionaryService
    {
        public RpcDictionaryService(ILogger<RpcDictionaryService> logger, IBus bus) 
            : base(logger, bus) { }

        public async Task<ExecutionResult<List<EducationDocumentTypeDTO>>> GetEducationDocumentTypesAsync()
        {
            ExecutionResult<GetDocumentTypesResponse> response
                = await RequestHandlerAsync<ExecutionResult<GetDocumentTypesResponse>, GetDocumentTypesRequest>(
                     new(), "GetEducationDocumentTypeFail");

            return ResponseHandler(response, educationDocumentType => educationDocumentType.DocumentTypes);
        }

        public async Task<ExecutionResult<List<FacultyDTO>>> GetFacultiesAsync()
        {
            ExecutionResult<GetFacultiesResponse> response
                = await RequestHandlerAsync<ExecutionResult<GetFacultiesResponse>, GetFacultiesRequest>(
                     new(), "GetFacultiesFail");

            return ResponseHandler(response, faculties => faculties.Faculties);
        }
    }
}
