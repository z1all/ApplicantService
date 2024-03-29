namespace UserService.Infrastructure.Identity.Configurations.Authorization
{
    internal class JwtOptions
    {
        public string SecretKey { get; set; } = null!;
        public int AccessTokenTimeLifeMinutes { get; set; }
        public int RefreshTokenTimeLifeDays { get; set; }
    }
}
