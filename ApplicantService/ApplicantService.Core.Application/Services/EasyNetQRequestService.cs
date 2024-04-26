﻿using ApplicantService.Core.Application.Interfaces.Services;
using ApplicantService.Core.Domain;
using Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService;
using Common.Models.Models;
using Common.ServiceBus.ServiceBusDTOs.FromDictionaryService.Requests;
using EasyNetQ;

namespace ApplicantService.Core.Application.Services
{
    public class EasyNetQRequestService : IRequestService
    {
        private readonly IBus _bus;

        public EasyNetQRequestService(IBus bus)
        {
            _bus = bus;
        }

        public async Task<ExecutionResult> CheckAdmissionStatusIsCloseAsync(Guid applicantId)
        {
            return await RequestHandler<ExecutionResult, CheckAdmissionStatusIsCloseRequest>(new()
            {
                ApplicantId = applicantId
            }, "CheckAdmissionStatusIsCloseFail");
        }

        public async Task<ExecutionResult> CheckManagerEditPermissionAsync(Guid applicantId, Guid managerId)
        {
            return await RequestHandler<ExecutionResult, CheckManagerEditPermissionRequest>(new()
            {
                ApplicantId = applicantId,
                ManagerId = managerId
            }, " CheckManagerEditPermissionFail");
        }

        public async Task<ExecutionResult<EducationDocumentTypeCache>> GetEducationDocumentTypeAsync(Guid documentId)
        {
            var response = await RequestHandler<ExecutionResult<GetEducationDocumentTypeResponse>, GetEducationDocumentTypeRequest>(new()
            {
                DocumentId = documentId,
            }, "GetEducationDocumentTypeFail");

            if (!response.IsSuccess)
            {
                return new() { Errors = response.Errors };
            }

            return new()
            {
                Result = new()
                {
                    Id = response.Result!.EducationDocumentType.Id,
                    Name = response.Result!.EducationDocumentType.Name,
                    Deprecated = response.Result!.Deprecated,
                }
            };
        }

        private async Task<TResponse> RequestHandler<TResponse, TRequest>(TRequest request, string keyError) where TResponse : ExecutionResult, new()
        {
            return await _bus.Rpc
                .RequestAsync<TRequest, TResponse>(request)
                .ContinueWith(task => 
                {
                    if (task.Status == TaskStatus.Canceled)
                    {
                        return (TResponse)Activator.CreateInstance(typeof(TResponse), keyError, "Unknown error!")!;
                    }

                    return task.Result;
                });
        }
    }
}
