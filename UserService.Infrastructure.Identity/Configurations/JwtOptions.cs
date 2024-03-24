namespace UserService.Infrastructure.Identity.Configurations
{
    internal class JwtOptions
    {
        public string SecretKey { get; set; } = null!;
        public int AccessTokenTimeLifeMinutes { get; set; }
        public int RefreshTokenTimeLifeDays { get; set; }
    }
}
