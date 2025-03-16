namespace Hospital.SharedKernel.Application.Models
{
    public class CallbackWrapper
    {
        public Func<Task> Callback { get; set; }

        public CallbackWrapper()
        {
            Callback = () => Task.CompletedTask;
        }
    }
}