namespace Common.ServiceBus.ServiceBusDTOs.FromApplicantService.Requests
{
    public class GetDocumentTypesRequest
    {
        public required List<Guid> DocumentTypesId { get; set; }
    }
}
