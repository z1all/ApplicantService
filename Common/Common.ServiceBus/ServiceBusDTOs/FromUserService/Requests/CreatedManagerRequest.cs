using Common.ServiceBus.ServiceBusDTOs.FromUserService.Base;

namespace Common.ServiceBus.ServiceBusDTOs.FromUserService.Requests
{
    public class CreatedManagerRequest : BaseUser
    {
        public Guid? FacultyId { get; init; } = null;
    }
}
