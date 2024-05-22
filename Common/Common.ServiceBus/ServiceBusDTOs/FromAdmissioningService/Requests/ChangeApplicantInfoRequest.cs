using Common.Models.Enums;

namespace Common.ServiceBus.ServiceBusDTOs.FromAdmissioningService.Requests
{
    public class ChangeApplicantInfoRequest
    {
        public required Guid ApplicantId { get; set; }
        public required Guid ManagerId { get; set; }

        public required DateOnly Birthday { get; set; }
        public required Gender Gender { get; set; }
        public required string Citizenship { get; set; }
        public required string PhoneNumber { get; set; } 
    }
}
