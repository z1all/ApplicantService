namespace Common.ServiceBus.ServiceBusDTOs.FromApplicantService
{
    public class GetApplicantResponse
    {
        public required Guid Id { get; set; }
        public required string FullName { get; set; }
        public required string Email { get; set; }
        public required List<Guid> AddedDocumentTypesId { get; set; }
    }
}
