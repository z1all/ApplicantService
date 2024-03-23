namespace UserService.Core.Application.DTOs
{
    public class TokenResponse
    {
        public string Refresh { get; set; } = null!;
        public string Access { get; set; } = null!;
    }
}
