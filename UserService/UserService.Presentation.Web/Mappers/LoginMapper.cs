using UserService.Core.Application.DTOs;
using Common.ServiceBus.ServiceBusDTOs.FromUserService.Requests;

namespace UserService.Presentation.Web.Mappers
{
    internal static class LoginMapper
    {
        public static LoginDTO ToLoginDTO(this ManagerLoginRequest login)
        {
            return new()
            {
                Email = login.Email,
                Password = login.Password,
            };
        }
    }
}
