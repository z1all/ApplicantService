namespace Common.ServiceBusDTOs.FromAdmissioningService
{
    public class CheckManagerEditPermissionRequest
    {
        public Guid ApplicantId { get; set; }
        public Guid ManagerId { get; set; }
    }
}
