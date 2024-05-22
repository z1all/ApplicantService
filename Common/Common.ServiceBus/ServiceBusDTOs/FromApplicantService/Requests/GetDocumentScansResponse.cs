using Common.Models.DTOs.Applicant;

namespace Common.ServiceBus.ServiceBusDTOs.FromApplicantService.Requests
{
    public class GetDocumentScansResponse
    {
        public required List<ScanInfo> Scans { get; set; }
    }
}
