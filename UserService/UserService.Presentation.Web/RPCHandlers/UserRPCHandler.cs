using UserService.Core.Application.Interfaces;
using Common.Models.Models;
using Common.ServiceBus.EasyNetQRPC;
using Common.ServiceBus.ServiceBusDTOs.FromUserService.Requests;
using EasyNetQ;

namespace UserService.Presentation.Web.RPCHandlers
{
    public class UserRPCHandler : BaseEasyNetQRPCHandler
    {
        public UserRPCHandler(ILogger<UserRPCHandler> logger, IServiceProvider serviceProvider, IBus bus) 
            : base(logger, serviceProvider, bus) { }

        public override void CreateRequestListeners()
        {
            _bus.Rpc.Respond<ChangeFullNameRequest, ExecutionResult>(async (request) =>
                await ExceptionHandlerAsync(async (service) => await ChangeProfileAsync(service, request)));

            _bus.Rpc.Respond<ChangeEmailRequest, ExecutionResult>(async (request) =>
                await ExceptionHandlerAsync(async (service) => await ChangeEmailAsync(service, request)));

            _bus.Rpc.Respond<ChangePasswordRequest, ExecutionResult>(async (request) =>
                await ExceptionHandlerAsync(async (service) => await ChangePasswordAsync(service, request)));
        }

        private async Task<ExecutionResult> ChangeProfileAsync(IServiceProvider service, ChangeFullNameRequest request)
        {
            var _profileService = service.GetRequiredService<IProfileService>();

            return await _profileService.ChangeProfileAsync(new() { NewFullName = request.NewFullName }, request.UserId, request.ManagerId);
        }

        private async Task<ExecutionResult> ChangeEmailAsync(IServiceProvider service, ChangeEmailRequest request)
        {
            var _profileService = service.GetRequiredService<IProfileService>();

            return await _profileService.ChangeEmailAsync(new() { NewEmail = request.NewEmail }, request.ManagerId);
        }

        private async Task<ExecutionResult> ChangePasswordAsync(IServiceProvider service, ChangePasswordRequest request)
        {
            var _profileService = service.GetRequiredService<IProfileService>();

            return await _profileService.ChangePasswordAsync(request.ChangePassword, request.UserId);
        }
    }
}
