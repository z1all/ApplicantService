using Common.Models.DTOs;

namespace Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService.Requests
{
    public class GetAdmissionsAsyncResponse
    {
        public required ApplicantAdmissionPagedDTO ApplicantAdmissionPaged {  get; set; }
    }
}
