using Hospital.SharedKernel.Infrastructure.Caching;
using StackExchange.Redis;
namespace Hospital.SharedKernel.Infrastructure.Redis
{
    public interface IRedisCache : IBaseCache
    {
        IConnectionMultiplexer GetInstance();

        Task<T> GetOrSetAsync<T>(string key, Func<Task<T>> valueFactory, TimeSpan? absoluteExpireTime = null, CancellationToken cancellationToken = default);

        Task<T> GetAsync<T>(string key, CancellationToken cancellationToken = default);

        Task<T> GetWihtoutPrefixAsync<T>(string key, CancellationToken cancellationToken = default);

        Task SetAsync(string key, object value, TimeSpan? absoluteExpireTime = null, CancellationToken cancellationToken = default);

        Task SetWithoutPrefixAsync(string key, object value, TimeSpan? absoluteExpireTime = null, CancellationToken cancellationToken = default);

        Task RemoveAsync(string key, CancellationToken cancellationToken = default);

        Task RemoveByPatternAsync(string pattern);
    }
}
