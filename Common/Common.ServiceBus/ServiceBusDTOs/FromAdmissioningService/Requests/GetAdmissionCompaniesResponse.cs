using Common.Models.DTOs.Admission;

namespace Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService.Requests
{
    public class GetAdmissionCompaniesResponse
    {
        public required List<AdmissionCompanyDTO> AdmissionCompanies { get; set; }
    }
}
