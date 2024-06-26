﻿using UserService.Core.Application.DTOs;
using UserService.Core.Application.Interfaces;
using UserService.Presentation.Web.Mappers;
using Common.Models.Models;
using Common.ServiceBus.ServiceBusDTOs.FromUserService.Requests;
using Common.ServiceBus.EasyNetQRPC;
using EasyNetQ;

namespace UserService.Presentation.Web.RPCHandlers
{
    public class ManagerRPCHandler : BaseEasyNetQRPCHandler
    {
        public ManagerRPCHandler(ILogger<ManagerRPCHandler> logger, IServiceProvider serviceProvider, IBus bus) : 
            base(logger, serviceProvider, bus) { }

        public override void CreateRequestListeners()
        {
            _bus.Rpc.Respond<ManagerLoginRequest, ExecutionResult<TokensResponse>>(async (request) =>
               await ExceptionHandlerAsync(async (service) => await ManagerLoginAsync(service, request)));

            _bus.Rpc.Respond<UpdateTokensRequest, ExecutionResult<TokensResponse>>(async (request) =>
              await ExceptionHandlerAsync(async (service) => await UpdateTokensAsync(service, request)));
            
            _bus.Rpc.Respond<LogoutRequest, ExecutionResult>(async (request) =>
              await ExceptionHandlerAsync(async (service) => await LogoutAsync(service, request)));

            _bus.Rpc.Respond<CreateNewManagerRequest, ExecutionResult>(async (request) =>
             await ExceptionHandlerAsync(async (service) => await CreateManagerAsync(service, request)));

            _bus.Rpc.Respond<DeleteManagerRequest, ExecutionResult>(async (request) =>
              await ExceptionHandlerAsync(async (service) => await DeleteManagerAsync(service, request)));
            
            _bus.Rpc.Respond<ChangeManagerRequest, ExecutionResult>(async (request) =>
              await ExceptionHandlerAsync(async (service) => await ChangeManagerAsync(service, request)));
        }

        private async Task<ExecutionResult<TokensResponse>> ManagerLoginAsync(IServiceProvider service, ManagerLoginRequest request)
        {
            var _authService = service.GetRequiredService<IAuthService>();

            ExecutionResult<TokensResponseDTO> tokenResponse = await _authService.ManagerLoginAsync(request.ToLoginDTO());

            return ResponseHandler(tokenResponse, tokens => tokens.ToTokenResponse());
        }

        private async Task<ExecutionResult<TokensResponse>> UpdateTokensAsync(IServiceProvider service, UpdateTokensRequest request)
        {
            var _authService = service.GetRequiredService<IAuthService>();

            ExecutionResult<TokensResponseDTO> tokenResponse 
                = await _authService.UpdateAccessTokenAsync(request.Refresh, request.AccessTokenJTI, request.UserId);

            return ResponseHandler(tokenResponse, tokens => tokens.ToTokenResponse());
        }

        private async Task<ExecutionResult> LogoutAsync(IServiceProvider service, LogoutRequest request)
        {
            var _authService = service.GetRequiredService<IAuthService>();

            return await _authService.LogoutAsync(request.AccessTokenJTI);
        }

        private async Task<ExecutionResult> CreateManagerAsync(IServiceProvider service, CreateNewManagerRequest request)
        {
            var _profileService = service.GetRequiredService<IProfileService>();

            return await _profileService.CreateManagerAsync(new ()
            {
                Email = request.Manager.Email,
                FullName = request.Manager.FullName,
                Password = request.Password,
                FacultyId = request.Manager.FacultyId,
            });
        }

        private async Task<ExecutionResult> DeleteManagerAsync(IServiceProvider service, DeleteManagerRequest request)
        {
            var _profileService = service.GetRequiredService<IProfileService>();

            return await _profileService.DeleteManagerAsync(request.ManagerId);
        }

        private async Task<ExecutionResult> ChangeManagerAsync(IServiceProvider service, ChangeManagerRequest request)
        {
            var _profileService = service.GetRequiredService<IProfileService>();

            return await _profileService.ChangeManagerAsync(new()
            {
                Id = request.ManagerId,
                FullName = request.Manager.FullName,
                Email = request.Manager.Email,
                FacultyId = request.Manager.FacultyId,
            });
        }
    }
}
