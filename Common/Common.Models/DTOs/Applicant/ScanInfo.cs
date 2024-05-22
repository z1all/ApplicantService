namespace Common.Models.DTOs.Applicant
{
    public class ScanInfo
    {
        public required Guid Id { get; set; }
        public required string Type { get; set; }
        public required string Name { get; set; }
    }
}
