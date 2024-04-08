namespace ApplicantService.Core.Application.DTOs
{
    public class ScanInfo
    {
        public required Guid Id { get; set; }
        public required string Type { get; set; }
        public required string Name { get; set; }
    }
}
