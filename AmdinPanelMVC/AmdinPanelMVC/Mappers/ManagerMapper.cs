using AmdinPanelMVC.DTOs;
using Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService.Requests;
using Common.ServiceBus.ServiceBusDTOs.FromUserService.Requests;

namespace AmdinPanelMVC.Mappers
{
    internal static class ManagerMapper
    {
        public static ManagerLoginRequest ToManagerLoginRequest(this LoginRequestDTO login)
        {
            return new()
            {
                Email = login.Email,
                Password = login.Password,
            };
        }

        public static ManagerDTO ToManagerDTO(this GetManagerProfileResponse manager)
        {
            return new()
            {
                Id = manager.Id,
                Email = manager.Email,
                FullName = manager.FullName,
                Faculty = manager.Faculty,
            };
        }
    }
}
