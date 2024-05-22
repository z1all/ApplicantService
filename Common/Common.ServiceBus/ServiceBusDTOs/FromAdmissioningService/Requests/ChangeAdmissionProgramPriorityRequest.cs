using Common.Models.DTOs.Admission;

namespace Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService.Requests
{
    public class ChangeAdmissionProgramPriorityRequest
    {
        public required Guid ApplicantId { get; set; }
        public required Guid ManagerId { get; set; }
        public required ChangePrioritiesApplicantProgramDTO ChangePriorities { get; set; }
    }
}
