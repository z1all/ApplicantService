using Common.Models.DTOs;

namespace Common.ServiceBus.ServiceBusDTOs.FromUserService.Requests
{
    public class ChangeManagerRequest
    {
        public required Guid ManagerId { get; set; }
        public required ManagerDTO Manager { get; set; }
    }
}
