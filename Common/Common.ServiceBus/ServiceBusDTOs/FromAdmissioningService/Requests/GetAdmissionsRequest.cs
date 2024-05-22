using Common.Models.DTOs.Admission;

namespace Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService.Requests
{
    public class GetAdmissionsRequest
    {
        public required ApplicantAdmissionFilterDTO ApplicantAdmissionFilter { get; set; }
        public required Guid ManagerId { get; set; }
    }
}
