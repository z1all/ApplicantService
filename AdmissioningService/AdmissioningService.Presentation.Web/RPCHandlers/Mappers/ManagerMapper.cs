using AdmissioningService.Core.Application.DTOs;
using Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService.Requests;

namespace AdmissioningService.Presentation.Web.RPCHandlers.Mappers
{
    public static class ManagerMapper
    {
        public static GetManagerProfileResponse ToGetManagerProfileResponse(this ManagerDTO manager)
        {
            return new()
            {
                Id = manager.Id,
                Email = manager.Email,
                FullName = manager.FullName,
                Faculty = manager.Faculty
            };
        }
    }
}
