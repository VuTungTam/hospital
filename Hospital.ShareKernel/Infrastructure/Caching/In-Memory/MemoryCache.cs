using Hospital.SharedKernel.Application.Consts;
using Hospital.SharedKernel.Infrastructure.Caching.Models;

namespace Hospital.SharedKernel.Caching.In_Memory
{
    public class MemoryCache : IMemoryCache
    {
        public TimeSpan? DefaultAbsoluteExpireTime => TimeSpan.FromSeconds(CacheManager.DefaultExpiriesInSeconds);

        public TimeSpan? DefaultSlidingExpireTime => null;

        public object Get(string key) => MemoryCacheHelper.Get(CacheManager.GetCombineKey(key));

        public void Remove(string key) => MemoryCacheHelper.Remove(CacheManager.GetCombineKey(key));

        public void Set(string key, object value, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpireTime = null)
        {
            var time = absoluteExpireTime ?? DefaultAbsoluteExpireTime;
            MemoryCacheHelper.Set(CacheManager.GetCombineKey(key), value, time);
        }
    }
}
