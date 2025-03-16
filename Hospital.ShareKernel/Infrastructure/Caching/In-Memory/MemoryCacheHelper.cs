using Hospital.SharedKernel.Infrastructure.Caching.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Hospital.SharedKernel.Caching.In_Memory
{
    public static class MemoryCacheHelper
    {
        private static readonly Microsoft.Extensions.Caching.Memory.MemoryCache _cacheInstance = new(new MemoryCacheOptions());
        
        public static object Get(string key) => _cacheInstance.Get(key);

        public static void Remove(string key) => _cacheInstance.Remove(key);

        public static void Set(string key, object value, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpireTime = null)
        {
            var time = absoluteExpireTime ?? TimeSpan.FromSeconds(CacheManager.DefaultExpiriesInSeconds);
            _cacheInstance.Set(key, value, time);
        }
    }
}
