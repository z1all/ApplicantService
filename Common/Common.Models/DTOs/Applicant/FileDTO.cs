namespace Common.Models.DTOs.Applicant
{
    public class FileDTO
    {
        public required string Name { get; set; }
        public required string Type { get; set; }
        public required byte[] File { get; set; }
    }
}
