namespace Common.ServiceBus.ServiceBusDTOs.FromUserService.Requests
{
    public class ChangeEmailRequest
    {
        public required Guid ManagerId { get; set; }
        public required string NewEmail { get; set; }
    }
}
