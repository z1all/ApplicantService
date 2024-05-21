namespace Common.ServiceBus.ServiceBusDTOs.FromApplicantService.Requests
{
    public class ChangePassportRequest
    {
        public required Guid ApplicantId { get; set; }
        public required Guid MangerId { get; set; }
        public required string SeriesNumber { get; set; }
        public required string BirthPlace { get; set; }
        public required DateOnly IssueYear { get; set; }
        public required string IssuedByWhom { get; set; }
    }
}
