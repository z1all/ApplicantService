namespace Common.ServiceBus.ServiceBusDTOs.FromUserService.Requests
{
    public class TokensResponse
    {
        public required string JwtToken { get; set; }
        public required string RefreshToken { get; set; }
    }
}
