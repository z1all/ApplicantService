using UserService.Core.Application.DTOs;
using UserService.Core.Application.Interfaces;
using Common.ServiceBus.ServiceBusDTOs.FromUserService;
using Common.Models.Models;
using EasyNetQ;

namespace UserService.Infrastructure.Identity.Services
{
    internal class EasyNetQRequestService : IRequestService
    {
        private readonly IBus _bus;

        public EasyNetQRequestService(IBus bus)
        {
            _bus = bus;
        }

        public async Task<ExecutionResult> CreateManagerAsync(Manager manager)
        {
            ExecutionResult result = await _bus.Rpc.RequestAsync<ManagerCreateRequest, ExecutionResult>(new()
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
            ExecutionResult result = await _bus.Rpc.RequestAsync<Guid, ExecutionResult>(managerId);

            return result;
        }
    }
}
