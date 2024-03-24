using StackExchange.Redis;
using UserService.Core.Application.Interfaces;

namespace UserService.Infrastructure.Identity.Services
{
    internal class TokenRedisService : ITokenDbService
    {
        private readonly IDatabase _redis;

        public TokenRedisService(IConnectionMultiplexer connectionMultiplexer)
        {
            _redis = connectionMultiplexer.GetDatabase();
        }
    }
}
