using EventManagment.Core.Application.Abstraction.Common.Contracts.Infrastracture;
using EventManagment.Shared.Settings;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Text.Json;

namespace EventManagment.Infrastructure.Caching_Service
{
    public class ResponseCacheService(IConnectionMultiplexer connectionMultiplexer, IOptions<RedisSettings> redisSettings) : IResponseCacheService
    {
        private readonly IDatabase _database = connectionMultiplexer.GetDatabase();
        private readonly RedisSettings _redisSettings = redisSettings.Value;
        public async Task CacheResponseAsync(string key, object response, TimeSpan timeToLive)
        {
            var serializedResponse = JsonSerializer.Serialize(response, new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
            await _database.StringSetAsync(key, serializedResponse, timeToLive);

        }

        public async Task<string?> GetCachedResponseAsync(string key)
        {
            var cachedResponse = await _database.StringGetAsync(key);
            if (cachedResponse.IsNullOrEmpty)
            {
                return null!;
            }
            return cachedResponse;
        }
    }
}
