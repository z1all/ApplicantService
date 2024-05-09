namespace Common.ServiceBus.ServiceBusDTOs.FromUserService.Requests
{
    public class UpdateTokensRequest
    {
        public required string Refresh { set; get; }
        public required Guid AccessTokenJTI { set; get; }
        public required Guid UserId { set; get; }
    }
}
