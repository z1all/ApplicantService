using UserService.Core.Application.DTOs;
using Common.ServiceBus.ServiceBusDTOs.FromUserService.Requests;

namespace UserService.Presentation.Web.Mappers
{
    internal static class TokenResponseMapper
    {
        public static TokensResponse ToTokenResponse(this TokensResponseDTO tokens)
        {
            return new()
            {
                JwtToken = tokens.Access,
                RefreshToken = tokens.Refresh,
            };
        }
    }
}
