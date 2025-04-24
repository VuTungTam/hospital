using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Infrastructure.Caching.Models;
using MassTransit.Internals;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Hospital.SharedKernel.Infrastructure.Redis
{
    public class RedisCache : IRedisCache
    {
        public TimeSpan? DefaultAbsoluteExpireTime => TimeSpan.FromSeconds(CacheManager.DefaultExpiriesInSeconds);

        private readonly IConnectionMultiplexer _redis;

        private readonly IDatabase _db;

        public RedisCache(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _db = redis.GetDatabase();
        }

        public IConnectionMultiplexer GetInstance() => _redis;

        public async Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            var value = await _db.StringGetAsync(CacheManager.GetCombineKey(key));

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
            var isCollection = typeof(T).IsGenericType && typeof(T).GetGenericTypeDefinition() == typeof(List<>);
            var isPagination = typeof(T).HasInterface<IPaginationResult>();

            if (data != null)
            {
                if (!isCollection && !isPagination)
                {
                    return data;
                }

                if (isCollection && ((System.Collections.ICollection)data).Count > 0)
                {
                    return data;
                }

                if (isPagination && (data as IPaginationResult).Total > 0)
                {
                    return data;
                }
            }

            data = await valueFactory();
            if (data != null)
            {
                if (
                    (!isCollection && !isPagination) ||
                    (isCollection && ((System.Collections.ICollection)data).Count > 0) ||
                    (isPagination && (data as IPaginationResult).Total > 0)
                )
                {
                    await SetAsync(key, data, absoluteExpireTime, cancellationToken: cancellationToken);
                }
            }
            return data;
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            await _db.KeyDeleteAsync(CacheManager.GetCombineKey(key));
        }

        public async Task SetAsync(string key, object value, TimeSpan? absoluteExpireTime = null, CancellationToken cancellationToken = default)
            => await _db.StringSetAsync(CacheManager.GetCombineKey(key), new RedisValue(JsonConvert.SerializeObject(value)), absoluteExpireTime ?? DefaultAbsoluteExpireTime);

        public async Task SetWithoutPrefixAsync(string key, object value, TimeSpan? absoluteExpireTime = null, CancellationToken cancellationToken = default)
            => await _db.StringSetAsync(key, new RedisValue(JsonConvert.SerializeObject(value)), absoluteExpireTime ?? DefaultAbsoluteExpireTime);

        public async Task RemoveByPatternAsync(string pattern)
        {
            var cPattern = CacheManager.GetCombineKey(pattern);
            var server = _redis.GetServers().First();
            var keys = server.Keys(pattern: cPattern).ToList();

            if (keys != null && keys.Any())
            {
                var tasks = keys.Select(key => _db.KeyDeleteAsync(key));
                await Task.WhenAll(tasks);
            }
        }
    }
}
