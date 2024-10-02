namespace Hospital.SharedKernel.Caching.In_Memory
{
    public interface IMemoryCache : IBaseCache
    {
        object Get(string key);

        void Set(string key, object value, TimeSpan? absoluteExpireTime = null, TimeSpan? slidingExpireTime = null);

        void Remove(string key);
    }
}
