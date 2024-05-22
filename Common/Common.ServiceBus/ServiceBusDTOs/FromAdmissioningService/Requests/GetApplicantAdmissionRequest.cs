namespace Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService.Requests
{
    public class GetApplicantAdmissionRequest
    {
        public required Guid ApplicantId { get; set; }
        public required Guid AdmissionId { get; set; }
    }
}
