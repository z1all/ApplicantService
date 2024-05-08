﻿using AmdinPanelMVC.DTOs;
using Common.Models.Models;

namespace AmdinPanelMVC.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ExecutionResult<ManagerDTO>> GetManagerProfileAsync(Guid managerId);
        Task<ExecutionResult<TokensResponseDTO>> LoginAsync(LoginRequestDTO login);
        Task<ExecutionResult> LogoutAsync(Guid accessTokenJTI);
        Task<ExecutionResult<TokensResponseDTO>> UpdateAccessTokenAsync(string refresh, Guid accessTokenJTI, Guid userId);
    }
}
