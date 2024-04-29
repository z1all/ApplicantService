using Common.ServiceBus.ServiceBusDTOs.FromUserService.Base;

namespace Common.ServiceBus.ServiceBusDTOs.FromUserService
{
    public class CreateManagerRequest : BaseUser
    {
        public Guid? FacultyId { get; init; } = null;
    }
}
