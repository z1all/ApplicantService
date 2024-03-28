using Common.ServiceBusDTOs;
using EasyNetQ;
using UserService.Core.Application.DTOs;
using UserService.Core.Application.Interfaces;
using UserService.Core.Application.Models;

namespace UserService.Infrastructure.Identity.Services
{
    internal class EasynetqSendNotification : ISendNotification
    {
        private readonly IBus _bus;

        public EasynetqSendNotification(IBus bus)
        {
            _bus = bus;
        }

        public async Task<ExecutionResult> CreatedApplicant(User user)
        {
            bool result = await _bus.PubSub.PublishAsync<UserNotification>(new() 
            { 
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
            }).ContinueWith(task => task.IsCompletedSuccessfully);

            return new(result);
        }

        public Task<ExecutionResult> CreatedManager(User user)
        {
            throw new NotImplementedException();
        }

        public Task<ExecutionResult> UpdatedUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
