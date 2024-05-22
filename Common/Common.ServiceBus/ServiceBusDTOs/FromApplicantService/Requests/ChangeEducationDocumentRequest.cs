namespace Common.ServiceBus.ServiceBusDTOs.FromApplicantService.Requests
{
    public class ChangeEducationDocumentRequest
    {
        public required Guid ApplicantId { get; set; }
        public required Guid DocumentId { get; set; }
        public required Guid ManagerId { get; set; }
        public required string Name { get; set; }
        public required Guid EducationDocumentTypeId { get; set; }
    }
}
