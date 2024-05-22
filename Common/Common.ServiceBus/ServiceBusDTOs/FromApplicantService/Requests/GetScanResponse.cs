using Common.Models.DTOs.Applicant;

namespace Common.ServiceBus.ServiceBusDTOs.FromApplicantService.Requests
{
    public class GetScanResponse
    {
        public required FileDTO File { get; set; }
    }
}
