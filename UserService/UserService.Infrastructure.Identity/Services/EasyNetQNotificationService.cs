using UserService.Core.Application.DTOs;
using UserService.Core.Application.Interfaces;
using Common.ServiceBus.ServiceBusDTOs.FromUserService;
using Common.ServiceBus.ServiceBusDTOs.FromUserService.Base;
using EasyNetQ;
using Common.Models.Models;
using Common.ServiceBus.ServiceBusDTOs.FromUserService.Notifications;

namespace UserService.Infrastructure.Identity.Services
{
    internal class EasyNetQNotificationService : INotificationService
    {
        private readonly IBus _bus;

        public EasyNetQNotificationService(IBus bus)
        {
            _bus = bus;
        }

        public async Task<ExecutionResult> CreatedApplicantAsync(UserDTO user)
        {
            bool result = await _bus.PubSub
                .PublishAsync(UserMapTo<ApplicantCreatedNotification>(user))
                .ContinueWith(task => task.IsCompletedSuccessfully);

            return GiveResult(result, "An error occurred when sending a notification about the creation of an applicant.");
        }

        public async Task<ExecutionResult> CreatedManagerAsync(UserDTO user, string password)
        {
            bool result = await _bus.PubSub
                .PublishAsync(new ManagerCreatedNotification()
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Password = password,
                }).ContinueWith(task => task.IsCompletedSuccessfully);

            return GiveResult(result, "An error occurred when sending a notification about the creation of a manager.");
        }

        public async Task<ExecutionResult> UpdatedUserAsync(UserDTO user)
        {
            bool result = await _bus.PubSub
                .PublishAsync(UserMapTo<UserUpdatedNotification>(user))
                .ContinueWith(task => task.IsCompletedSuccessfully);

            return GiveResult(result, "Error when sending a notification that the user has updated.");
        }

        private ExecutionResult GiveResult(bool result, string errorMassage)
        {
            if (!result)
            {
                return new("SendNotificationFail", errorMassage);
            }
            return new(isSuccess: true);
        }

        private T UserMapTo<T>(UserDTO user) where T : BaseUser, new()
        {
            return new T()
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
            };
        }
    }
}
