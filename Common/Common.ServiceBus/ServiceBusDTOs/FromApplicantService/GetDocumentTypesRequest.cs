namespace Common.ServiceBus.ServiceBusDTOs.FromApplicantService
{
    public class GetDocumentTypesRequest
    {
        public required List<Guid> DocumentTypesId { get; set; }
    }
}
