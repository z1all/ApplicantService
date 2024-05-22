namespace Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService.Requests
{
    public class DeleteProgramFromCurrentAdmissionRequest
    {
        public required Guid ApplicantId { get; set; }
        public required Guid ProgramId { get; set; }
        public required Guid ManagerId { get; set; }
    }
}
