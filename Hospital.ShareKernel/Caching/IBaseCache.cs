namespace Hospital.SharedKernel.Caching
{
    public interface IBaseCache
    {
        TimeSpan? DefaultAbsoluteExpireTime { get; }//Thời gian hết hạn tuyệt đối

        TimeSpan? DefaultSlidingExpireTime { get; }// Thời gian hết hạn sẽ được làm mới khi có truy cập
    }
}
