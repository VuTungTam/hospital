namespace Hospital.SharedKernel.Domain.Events.BaseEvents
{
    public class DomainEvent : Message
    {
        public Guid EventId { get; protected set; }

        public DateTime Timestamp { get; protected set; }

        public string Body { get; protected set; }

        public string EventType => GetType().Name;

        public DomainEvent(Guid eventId, string body)
        {
            EventId = eventId;
            Body = body;
            Timestamp = DateTime.Now;
        }
    }
}
