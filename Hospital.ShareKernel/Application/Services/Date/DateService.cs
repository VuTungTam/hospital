namespace Hospital.SharedKernel.Application.Services.Date
{
    public class DateService : IDateService
    {
        private long _differenceMilliseconds;

        public long DifferenceMilliseconds => _differenceMilliseconds;

        public DateTime Unix => new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public void SetDifference(long differenceMilliseconds)
        {
            if (_differenceMilliseconds == 0)
            {
                _differenceMilliseconds = differenceMilliseconds;
            }
        }

        public DateTime GetClientTime()
        {
            return DateTime.Now.AddMilliseconds(_differenceMilliseconds * -1);
        }
    }
}
