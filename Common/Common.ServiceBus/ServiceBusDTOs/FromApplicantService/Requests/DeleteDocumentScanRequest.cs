namespace Common.ServiceBus.ServiceBusDTOs.FromApplicantService.Requests
{
    public class DeleteDocumentScanRequest
    {
        public required Guid ApplicantId { get; set; }
        public required Guid DocumentId { get; set; }
        public required Guid ScanId { get; set; }
        public required Guid ManagerId { get; set; }
    }
}
