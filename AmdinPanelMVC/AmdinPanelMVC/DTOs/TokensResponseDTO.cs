namespace AmdinPanelMVC.DTOs
{
    public class TokensResponseDTO
    {
        public required string JwtToken { get; set; }   
        public required string RefreshToken { get; set; }   
    }
}
