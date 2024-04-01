namespace Common.Configurations.Authorization
{
    public class JwtOptions
    {
        public string SecretKey { get; set; } = null!;
        public int AccessTokenTimeLifeMinutes { get; set; }
        public int RefreshTokenTimeLifeDays { get; set; }
    }
}
