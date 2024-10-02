using MediatR;

namespace Hospital.SharedKernel.Domain.Events.BaseEvents
{
    public interface IDomainEvent : INotification
    {
        string EventId { get; set; }

        DateTime Timestamp { get; set; }

        object Body { get; set; }

        string EventType { get; }
    }
}
