namespace Hospital.SharedKernel.Presentations.SignalR.Models
{
    public class SignalRMessage
    {
        public int Type { get; set; }

        public object Data { get; set; }

        public string Message { get; set; }
    }
}
