namespace Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService.Requests
{
    public class CheckPermissionsRequest
    {
        public required Guid ApplicantId { get; set; }
        public required Guid? ManagerId { get; set; }
    }
}
