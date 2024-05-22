namespace Common.ServiceBus.ServiceBusDTOs.FromApplicantService.Requests
{
    public class GetEducationDocumentRequest
    {
        public required Guid ApplicantId { get; set; }
        public required Guid DocumentId { get; set; }
    }
}
