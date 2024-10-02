using Hospital.SharedKernel.Application.Consts;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Hospital.SharedKernel.Infrastructure.Redis
{
    public class ConnectionMultiplexerRedis : IRedisCache
    {
        public TimeSpan? DefaultAbsoluteExpireTime => TimeSpan.FromSeconds(BaseCacheTime.Default);

        public TimeSpan? DefaultSlidingExpireTime => null;

        private readonly IConnectionMultiplexer _redis;

        private readonly IDatabase _db;

        public ConnectionMultiplexerRedis(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _db = redis.GetDatabase();
        }

        public IConnectionMultiplexer GetInstance() => _redis;

        public async Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            var value = await _db.StringGetAsync(BaseCacheKeys.GetCombineKey(key));

            if (!value.HasValue)
            {
                return default;
            }
            return JsonConvert.DeserializeObject<T>(value.ToString());
        }

        public async Task<T> GetWihtoutPrefixAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            var value = await _db.StringGetAsync(key);

            if (!value.HasValue)
            {
                return default;
            }
            return JsonConvert.DeserializeObject<T>(value.ToString());
        }

        public async Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> valueFactory, TimeSpan? absoluteExpireTime = null, CancellationToken cancellationToken = default)
        {
            var data = await GetAsync<T>(key, cancellationToken);
            var isList = typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>);
            if (data != null)
            {
                if (!isList || isList && ((System.Collections.ICollection)data).Count > 0)
                {
                    return data;
                }
            }

            data = await valueFactory();
            if (data != null)
            {
                if (!isList || isList && ((System.Collections.ICollection)data).Count > 0)
                {
                    await SetAsync(key, data, absoluteExpireTime, cancellationToken: cancellationToken);
                }
            }
            return data;
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
            => await _db.KeyDeleteAsync(BaseCacheKeys.GetCombineKey(key));

        public async Task SetAsync(string key, object value, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpireTime = null, CancellationToken cancellationToken = default)
            => await _db.StringSetAsync(BaseCacheKeys.GetCombineKey(key), new RedisValue(JsonConvert.SerializeObject(value)), absoluteExpireTime ?? DefaultAbsoluteExpireTime);

        public async Task SetWithoutPrefixAsync(string key, object value, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpireTime = null, CancellationToken cancellationToken = default)
            => await _db.StringSetAsync(key, new RedisValue(JsonConvert.SerializeObject(value)), absoluteExpireTime ?? DefaultAbsoluteExpireTime);

        public async Task RemoveByPatternAsync(string pattern)
        {
            var server = _redis.GetServers().First();
            var keys = server.Keys(pattern: pattern).ToList();

            if (keys != null && keys.Any())
            {
                var tasks = keys.Select(key => _db.KeyDeleteAsync(key));
                await Task.WhenAll(tasks);
            }
        }
    }
}
