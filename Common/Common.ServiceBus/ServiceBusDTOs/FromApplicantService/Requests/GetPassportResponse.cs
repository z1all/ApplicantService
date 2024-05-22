using Common.Models.DTOs.Applicant;

namespace Common.ServiceBus.ServiceBusDTOs.FromApplicantService.Requests
{
    public class GetPassportResponse
    {
        public required PassportInfo Passport { get; set; }
    }
}
