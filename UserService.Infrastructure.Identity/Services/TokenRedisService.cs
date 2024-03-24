using Microsoft.Extensions.Options;
using StackExchange.Redis;
using UserService.Core.Application.Interfaces;
using UserService.Infrastructure.Identity.Configurations;

namespace UserService.Infrastructure.Identity.Services
{
    internal class TokenRedisService : ITokenDbService
    {
        private readonly IDatabase _redis;
        private readonly TimeSpan expiration;

        public TokenRedisService(IConnectionMultiplexer connectionMultiplexer, IOptions<JwtOptions> jwtOptions)
        {
            _redis = connectionMultiplexer.GetDatabase();

            int timeLifeDays = jwtOptions.Value.RefreshTokenTimeLifeDays;
            expiration = new TimeSpan(timeLifeDays, 0, 0, 0);
        }

        public async Task<bool> SaveTokens(string refreshToken, Guid accessTokenJTI)
        {
            bool result = await _redis.StringSetAsync(refreshToken, accessTokenJTI.ToString(), expiration);

            return result;  
        }
    }
}
