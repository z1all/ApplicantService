namespace Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService.Requests
{
    public class RejectApplicantAdmissionRequest
    {
        public required Guid MangerId { get; set; }
        public required Guid AdmissionId { get; set; }
    }
}
