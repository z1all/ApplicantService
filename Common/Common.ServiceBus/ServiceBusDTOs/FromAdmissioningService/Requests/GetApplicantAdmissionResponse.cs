using Common.Models.DTOs.Admission;

namespace Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService.Requests
{
    public class GetApplicantAdmissionResponse
    {
        public required ApplicantAdmissionDTO ApplicantAdmission { get; set; }
    }
}
