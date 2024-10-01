using Hospital.SharedKernel.Application.Consts;
using Microsoft.Extensions.Caching.Memory;

namespace Hospital.SharedKernel.Caching.In_Memory
{
    public static class MemoryCacheHelper
    {
        private static Microsoft.Extensions.Caching.Memory.MemoryCache _cacheInstance = new Microsoft.Extensions.Caching.Memory.MemoryCache(new MemoryCacheOptions());

        public static object Get(string key) => _cacheInstance.Get(key);

        public static void Remove(string key) => _cacheInstance.Remove(key);

        public static void Set(string key, object value, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpireTime = null)
        {
            var time = absoluteExpireTime ?? TimeSpan.FromSeconds(BaseCacheTime.Default);
            _cacheInstance.Set(key, value, time);
        }
    }
}
