using Common.ServiceBusDTOs.FromUserService.Base;

namespace Common.ServiceBusDTOs.FromUserService
{
    public class ManagerCreateRequest : BaseUser
    {
        public Guid? FacultyId { get; init; } = null;
    }
}
