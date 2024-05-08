using AmdinPanelMVC.DTOs;
using AmdinPanelMVC.Mappers;
using AmdinPanelMVC.Services.Base;
using AmdinPanelMVC.Services.Interfaces;
using Common.Models.Models;
using Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService.Requests;
using Common.ServiceBus.ServiceBusDTOs.FromUserService.Requests;
using EasyNetQ;

namespace AmdinPanelMVC.Services
{
    public class RpcAuthService : BaseRpcService, IAuthService
    {
        public RpcAuthService(IBus bus) : base(bus) { }

        public async Task<ExecutionResult<ManagerDTO>> GetManagerProfileAsync(Guid managerId)
        {
            ExecutionResult<GetManagerProfileResponse> response 
                = await RequestHandlerAsync<ExecutionResult<GetManagerProfileResponse>, GetManagerProfileRequest>(
                     new() { ManagerId = managerId }, "GetManagerRequestFail");

            return ResponseHandler(response, manager => manager.ToManagerDTO());
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
