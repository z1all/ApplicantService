namespace Common.ServiceBus.ServiceBusDTOs.FromUserService.Requests
{
    public class ChangeFullNameRequest
    {
        public required Guid ManagerId { get; set; }
        public required string NewFullName { get; set; }
    }
}
