using Common.Models.DTOs;

namespace Common.ServiceBus.ServiceBusDTOs.FromUserService.Requests
{
    public class CreateNewManagerRequest
    {
        public required ManagerDTO Manager { get; set; }
        public required string Password { get; set; }
    }
}
