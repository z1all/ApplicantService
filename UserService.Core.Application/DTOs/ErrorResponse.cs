namespace UserService.Core.Application.DTOs
{
    public class ErrorResponse
    {
        public string Error { get; set; } = null!;
        public string? Error_description { get; set; }
    }
}
