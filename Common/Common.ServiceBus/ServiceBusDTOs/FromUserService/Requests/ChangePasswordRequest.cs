using Common.Models.DTOs.User;

namespace Common.ServiceBus.ServiceBusDTOs.FromUserService.Requests
{
    public class ChangePasswordRequest
    {
        public required Guid UserId { get; set; }
        public required ChangePasswordDTO ChangePassword { get; set; }
    }
}
