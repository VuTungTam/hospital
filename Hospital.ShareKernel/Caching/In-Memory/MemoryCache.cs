using Hospital.SharedKernel.Application.Consts;

namespace Hospital.SharedKernel.Caching.In_Memory
{
    public class MemoryCache : IMemoryCache
    {
        public TimeSpan? DefaultAbsoluteExpireTime => TimeSpan.FromSeconds(BaseCacheTime.Default);

        public TimeSpan? DefaultSlidingExpireTime => null;

        public object Get(string key) => MemoryCacheHelper.Get(BaseCacheKeys.GetCombineKey(key));

        public void Remove(string key) => MemoryCacheHelper.Remove(BaseCacheKeys.GetCombineKey(key));

        public void Set(string key, object value, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpireTime = null)
        {
            var time = absoluteExpireTime ?? DefaultAbsoluteExpireTime;
            MemoryCacheHelper.Set(BaseCacheKeys.GetCombineKey(key), value, time);
        }
    }
}
