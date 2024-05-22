using Common.Models.DTOs.Admission;

namespace Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService.Requests
{
    public class GetAdmissionsResponse
    {
        public required ApplicantAdmissionPagedDTO ApplicantAdmissionPaged {  get; set; }
    }
}
