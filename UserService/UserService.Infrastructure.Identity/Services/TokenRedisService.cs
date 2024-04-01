using Microsoft.Extensions.Options;
using UserService.Core.Application.Interfaces;
using Common.Configurations.Authorization;
using StackExchange.Redis;

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

        public async Task<bool> RemoveTokensAsync(Guid accessTokenJTI)
        {
            bool result = await _redis.KeyDeleteAsync(accessTokenJTI.ToString());

            return result;
        }

        public async Task<bool> SaveTokensAsync(string refreshToken, Guid accessTokenJTI)
        {
            bool keyExists = await _redis.KeyExistsAsync(accessTokenJTI.ToString());
            if (keyExists) return false;
            
            bool result = await _redis.StringSetAsync(accessTokenJTI.ToString(), refreshToken, expiration);

            return result;  
        }

        public async Task<bool> TokensExist(string refresh, Guid accessTokenJTI)
        {
            RedisValue result = await _redis.StringGetAsync(accessTokenJTI.ToString());

            if (result.IsNullOrEmpty) return false;
            return result == refresh;
        }
    }
}
