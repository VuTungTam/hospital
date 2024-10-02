using Hospital.SharedKernel.Domain.Events.BaseEvents;

namespace Hospital.SharedKernel.Domain.Events.Interfaces
{
    public interface IEventDispatcher
    {
        Task FireEventAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IDomainEvent;
    }
}
