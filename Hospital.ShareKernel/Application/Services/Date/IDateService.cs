namespace Hospital.SharedKernel.Application.Services.Date
{
    public interface IDateService
    {
        /// <summary>
        /// Server - Client = Difference
        /// </summary>
        long DifferenceMilliseconds { get; }

        DateTime Unix { get; }

        void SetDifference(long timeDifference);

        DateTime GetClientTime();
    }
}
