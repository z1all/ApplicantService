using Common.Models.DTOs.Applicant;

namespace Common.ServiceBus.ServiceBusDTOs.FromApplicantService.Requests
{
    public class AddDocumentScanRequest
    {
        public required Guid ApplicantId { get; set; }
        public required Guid DocumentId { get; set; }
        public required Guid ManagerId { get; set; }
        public required FileDTO File { get; set; }
    }
}
