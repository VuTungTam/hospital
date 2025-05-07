namespace Hospital.SharedKernel.Presentations.SignalR.Models
{
    public class OnlineModel
    {
        public int AnonymousCount { get; set; }

        public int EmployeeCount { get; set; }

        public int CustomerCount { get; set; }

        public int Total => EmployeeCount + CustomerCount + AnonymousCount;
    }
}
