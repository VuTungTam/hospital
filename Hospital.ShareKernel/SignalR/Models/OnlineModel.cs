namespace Hospital.SharedKernel.SignalR.Models
{
    public class OnlineModel
    {
        public int AnonymousCount { get; set; }

        public int EmployeeCount { get; set; }

        public int CustomerCount { get; set; }

        public long Total => EmployeeCount + CustomerCount + AnonymousCount;
    }
}
