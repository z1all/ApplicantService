using Common.ServiceBus.ServiceBusDTOs.FromUserService.Base;

namespace Common.ServiceBus.ServiceBusDTOs.FromUserService
{
    public class ManagerCreatedNotification : BaseUser
    {
        public required string Password { get; set; }
    }
}
