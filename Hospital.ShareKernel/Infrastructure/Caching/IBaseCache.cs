namespace Hospital.SharedKernel.Infrastructure.Caching
{
    public interface IBaseCache
    {
        TimeSpan? DefaultAbsoluteExpireTime { get; }
    }
}
