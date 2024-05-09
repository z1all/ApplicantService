using AmdinPanelMVC.DTOs;
using Common.ServiceBus.ServiceBusDTOs.FromUserService.Requests;

namespace AmdinPanelMVC.Mappers
{
    public static class TokensResponseMapper
    {
        public static TokensResponseDTO ToTokensResponseDTO(this TokensResponse login)
        {
            return new()
            {
                JwtToken = login.JwtToken,
                RefreshToken = login.RefreshToken,
            };
        }
    }
}
