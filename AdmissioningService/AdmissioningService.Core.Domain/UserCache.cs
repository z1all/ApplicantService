using Common.Repositories;

namespace AdmissioningService.Core.Domain
{
    public class UserCache : BaseEntity
    {
        public required string FullName { get; set; }
        public required string Email { get; set; }
    }
}
