using Common.Models.Enums;

namespace Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService.Requests
{
    public class ChangeAdmissionStatusRequest
    {
        public required ManagerChangeAdmissionStatus NewStatus { get; set; }
        public required Guid AdmissionId { get; set; }
        public required Guid ManagerId { get; set; }
    }
}
