using AmdinPanelMVC.DTOs;
using AmdinPanelMVC.Mappers;
using AmdinPanelMVC.Services.Base;
using AmdinPanelMVC.Services.Interfaces;
using Common.Models.DTOs.Admission;
using Common.Models.DTOs.User;
using Common.Models.Models;
using Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService.Requests;
using Common.ServiceBus.ServiceBusDTOs.FromUserService.Requests;
using EasyNetQ;

namespace AmdinPanelMVC.Services
{
    public class RpcAuthService : BaseRpcService, IAuthService
    {
        public RpcAuthService(ILogger<RpcAuthService> logger, IBus bus) 
            : base(logger, bus) { }

        public async Task<ExecutionResult<ManagerProfileDTO>> GetManagerProfileAsync(Guid managerId)
        {
            ExecutionResult<GetManagerProfileResponse> response 
                = await RequestHandlerAsync<ExecutionResult<GetManagerProfileResponse>, GetManagerProfileRequest>(
                     new() { ManagerId = managerId }, "GetManagerRequestFail");

            return ResponseHandler(response, manager => manager.Manager);
        }

        public async Task<ExecutionResult> ChangeFullNameAsync(Guid userId, string newFullName, Guid? managerId)
        {
            return await RequestHandlerAsync<ExecutionResult, ChangeFullNameRequest>(
                new() { UserId = userId, NewFullName = newFullName, ManagerId = managerId }, "ChangeFullNameFail");
        }

        public async Task<ExecutionResult> ChangeEmailAsync(Guid userId, string newEmail)
        {
            return await RequestHandlerAsync<ExecutionResult, ChangeEmailRequest>(
                new() { ManagerId = userId, NewEmail = newEmail }, "ChangeEmailFail");
        }

        public async Task<ExecutionResult> ChangePasswordAsync(Guid userId, ChangePasswordDTO changePassword)
        {
            return await RequestHandlerAsync<ExecutionResult, ChangePasswordRequest>(
                new() { UserId = userId, ChangePassword = changePassword }, "ChangePasswordFail");
        }

        public async Task<ExecutionResult<TokensResponseDTO>> LoginAsync(LoginRequestDTO login)
        {
            ExecutionResult<TokensResponse> response = await RequestHandlerAsync<ExecutionResult<TokensResponse>, ManagerLoginRequest>(
                    login.ToManagerLoginRequest(), "LoginRequestFail");

            return ResponseHandler(response, tokens => tokens.ToTokensResponseDTO());
        }

        public async Task<ExecutionResult> LogoutAsync(Guid accessTokenJTI)
        {
            return await RequestHandlerAsync<ExecutionResult, LogoutRequest>(
                new() { AccessTokenJTI = accessTokenJTI }, "LogoutRequestFail");
        }

        public async Task<ExecutionResult<TokensResponseDTO>> UpdateAccessTokenAsync(string refresh, Guid accessTokenJTI, Guid userId)
        {
            ExecutionResult<TokensResponse> response = await RequestHandlerAsync<ExecutionResult<TokensResponse>, UpdateTokensRequest>(new()
            {
                Refresh = refresh,
                AccessTokenJTI = accessTokenJTI,
                UserId = userId
            }, "UpdateTokensRequestFail");

            return ResponseHandler(response, tokens => tokens.ToTokensResponseDTO());
        }
    }
}
