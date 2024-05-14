using Common.Models.DTOs;

namespace Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService.Requests
{
    public class GetAdmissionsAsyncRequest
    {
        public required ApplicantAdmissionFilterDTO ApplicantAdmissionFilter { get; set; }
        public required Guid ManagerId { get; set; }
    }
}
