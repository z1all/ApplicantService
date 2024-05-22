namespace Common.ServiceBus.ServiceBusDTOs.FromUserService.Requests
{
    public class ManagerLoginRequest
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
