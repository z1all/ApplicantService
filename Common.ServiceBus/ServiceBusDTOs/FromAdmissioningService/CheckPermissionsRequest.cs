namespace Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService
{
    public class CheckPermissionsRequest
    {
        public required Guid ApplicantId { get; set; }
        public required Guid? ManagerId { get; set; }
    }
}
