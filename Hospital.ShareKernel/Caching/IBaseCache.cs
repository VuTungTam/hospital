namespace Hospital.SharedKernel.Caching
{
    public interface IBaseCache
    {
        TimeSpan? DefaultAbsoluteExpireTime { get; }

        TimeSpan? DefaultSlidingExpireTime { get; }
    }
}
