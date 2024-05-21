using Common.Models.DTOs.Applicant;

namespace Common.ServiceBus.ServiceBusDTOs.FromApplicantService.Requests
{
    public class GetApplicantInfoResponse
    {
        public required ApplicantInfo ApplicantInfo { get; set; }
    }
}
