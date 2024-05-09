namespace Common.ServiceBus.ServiceBusDTOs.FromUserService.Requests
{
    public class LogoutRequest
    {
        public required Guid AccessTokenJTI { get; set; }
    }
}
