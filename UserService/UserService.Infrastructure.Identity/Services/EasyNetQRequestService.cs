using UserService.Core.Application.DTOs;
using UserService.Core.Application.Interfaces;
using Common.Models.Models;
using EasyNetQ;
using Common.ServiceBus.ServiceBusDTOs.FromUserService.Requests;
using Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService.Requests;

namespace UserService.Infrastructure.Identity.Services
{
    internal class EasyNetQRequestService : IRequestService
    {
        private readonly IBus _bus;

        public EasyNetQRequestService(IBus bus)
        {
            _bus = bus;
        }

        public async Task<ExecutionResult> CheckPermissionsAsync(Guid applicantId, Guid? managerId)
        {
            return await RequestHandlerAsync<ExecutionResult, CheckPermissionsRequest>(new()
            {
                ApplicantId = applicantId,
                ManagerId = managerId
            }, "CheckPermissionsFail");
        }

        public async Task<ExecutionResult> CreateManagerAsync(Manager manager)
        {
            ExecutionResult result = await _bus.Rpc.RequestAsync<Common.ServiceBus.ServiceBusDTOs.FromUserService.Requests.CreateManagerRequest, ExecutionResult>(new()
            {
                Id = manager.Id,
                Email = manager.Email,
                FullName = manager.FullName,
                FacultyId = manager.FacultyId,
            }).ContinueWith<ExecutionResult>(task =>
            {
                if (task.Status == TaskStatus.Canceled)
                {
                    return new("CreateManagerFail", "Unknown error!");
                }

                return task.Result;
            });

            return result;
        }

        public async Task<ExecutionResult> DeleteManagerAsync(Guid managerId)
        {
            ExecutionResult result = await _bus.Rpc.RequestAsync<DeleteManagerRequest, ExecutionResult>(new() { ManagerId = managerId });

            return result;
        }

        private async Task<TResponse> RequestHandlerAsync<TResponse, TRequest>(TRequest request, string keyError) where TResponse : ExecutionResult, new()
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
