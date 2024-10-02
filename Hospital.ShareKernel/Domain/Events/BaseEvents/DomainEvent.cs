namespace Hospital.SharedKernel.Domain.Events.BaseEvents
{
    public class DomainEvent : IDomainEvent
    {
        public string EventId { get; set; }

        public DateTime Timestamp { get; set; }

        public object Body { get; set; }

        public string EventType => GetType().Name;

        public DomainEvent()
        {
            EventId = Guid.NewGuid().ToString();
            Timestamp = DateTime.UtcNow;
        }
    }
}
