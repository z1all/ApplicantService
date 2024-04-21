namespace Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService
{
    public class ManagerAppointedNotification
    {
        public required Guid ManagerId { get; set; }
        public required string ManagerFullName { get; set; }
        public required string ManagerEmail { get; set; }

        public required Guid ApplicantId { get; set; }
        public required string ApplicantFullName { get; set; }
        public required string ApplicantEmail { get; set; }
    }
}
