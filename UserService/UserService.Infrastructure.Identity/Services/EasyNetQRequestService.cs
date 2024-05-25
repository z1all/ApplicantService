using UserService.Core.Application.DTOs;
using UserService.Core.Application.Interfaces;
using Common.Models.Models;
using Common.ServiceBus.ServiceBusDTOs.FromUserService.Requests;
using Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService.Requests;
using Common.ServiceBus.EasyNetQRPC;
using EasyNetQ;

namespace UserService.Infrastructure.Identity.Services
{
    internal class EasyNetQRequestService : BaseEasyNetQRPCustomer, IRequestService
    {
        public EasyNetQRequestService(IBus bus) : base(bus) { }

        public async Task<ExecutionResult> ChangeManagerAsync(Manager manager)
        {
            return await RequestHandlerAsync<ExecutionResult, ChangedManagerRequest>(
                new()
                {
                    ManagerId = manager.Id,
                    Manager = new()
                    {
                        Email = manager.Email,
                        FullName = manager.FullName,
                        FacultyId = manager.FacultyId,
                    }
                }, "ChangeManagerFail");
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
            ExecutionResult result = await _bus.Rpc.RequestAsync<CreatedManagerRequest, ExecutionResult>(new()
            {
                ManagerId = manager.Id,
                Manager = new()
                {
                    Email = manager.Email,
                    FullName = manager.FullName,
                    FacultyId = manager.FacultyId,
                }
            }).ContinueWith<ExecutionResult>(task =>
            {
                if (task.Status == TaskStatus.Canceled)
                {
                    return new(StatusCodeExecutionResult.InternalServer, "CreateManagerFail", "Unknown error!");
                }

                return task.Result;
            });

            return result;
        }

        public async Task<ExecutionResult> DeleteManagerAsync(Guid managerId)
        {
            ExecutionResult result = await _bus.Rpc.RequestAsync<DeletedManagerRequest, ExecutionResult>(new() { ManagerId = managerId });

            return result;
        }

        //private async Task<TResponse> RequestHandlerAsync<TResponse, TRequest>(TRequest request, string keyError) where TResponse : ExecutionResult, new()
        //{
        //    return await _bus.Rpc
        //        .RequestAsync<TRequest, TResponse>(request)
        //        .ContinueWith(task =>
        //        {
        //            if (task.Status == TaskStatus.Canceled)
        //            {
        //                return (TResponse)Activator.CreateInstance(typeof(TResponse), keyError, "Unknown error!")!;
        //            }

        //            return task.Result;
        //        });
        //}
    }
}
