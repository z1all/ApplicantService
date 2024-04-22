using Common.Models.Enums;

namespace Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService
{
    public class AdmissionStatusUpdatedNotification
    {
        public required Guid Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required AdmissionStatus NewStatus { get; set; }
    }
}
