namespace Common.ServiceBus.ServiceBusDTOs.FromApplicantService.Requests
{
    public class GetDocumentScansRequest
    {
        public required Guid ApplicantId { get; set; }
        public required Guid DocumentId { get; set; }
    }
}
