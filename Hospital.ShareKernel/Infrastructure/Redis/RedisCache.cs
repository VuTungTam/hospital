using Hospital.SharedKernel.Application.Consts;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Hospital.SharedKernel.Infrastructure.Redis
{
    public class RedisCache : IRedisCache
    {
        private readonly IDistributedCache _cacheInstance;

        public TimeSpan? DefaultAbsoluteExpireTime => TimeSpan.FromSeconds(BaseCacheTime.Default);

        public TimeSpan? DefaultSlidingExpireTime => null;

        public RedisCache(IDistributedCache cache)
        {
            _cacheInstance = cache;
        }

        public IDistributedCache GetInstance() => _cacheInstance;

        IConnectionMultiplexer IRedisCache.GetInstance()
        {
            throw new NotImplementedException();
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

        public async Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            var jsonData = await _cacheInstance.GetStringAsync(BaseCacheKeys.GetCombineKey(key), cancellationToken);
            if (jsonData is null)
            {
                return default;
            }

            return JsonConvert.DeserializeObject<T>(jsonData);
        }

        public string GetString(string key) => _cacheInstance.GetString(BaseCacheKeys.GetCombineKey(key));

        public async Task<string> GetStringAsync(string key, CancellationToken cancellationToken = default)
            => await _cacheInstance.GetStringAsync(BaseCacheKeys.GetCombineKey(key), cancellationToken) ?? string.Empty;

        public async Task SetAsync(string key, object value, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpireTime = null, CancellationToken cancellationToken = default)
        {
            string jsonData;
            if (value?.GetType() == typeof(string))
            {
                jsonData = value.ToString();
            }
            else
            {
                jsonData = JsonConvert.SerializeObject(value);
            }
            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = absoluteExpireTime ?? DefaultAbsoluteExpireTime,
                SlidingExpiration = slidingExpireTime ?? DefaultSlidingExpireTime
            };

            await _cacheInstance.SetStringAsync(BaseCacheKeys.GetCombineKey(key), jsonData, options, cancellationToken);
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
            => await _cacheInstance.RemoveAsync(BaseCacheKeys.GetCombineKey(key), cancellationToken);

        public Task RemoveByPatternAsync(string pattern)
        {
            throw new NotImplementedException();
        }

        public Task<T> GetWihtoutPrefixAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task SetWithoutPrefixAsync(string key, object value, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpireTime = null, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
