using Hospital.SharedKernel.Domain.Events.BaseEvents;

namespace Hospital.SharedKernel.Domain.Entities.Interfaces
{
    public interface IBaseEntity<TKey>
    {
        TKey Id { get; set; }
        IReadOnlyCollection<DomainEvent> DomainEvents { get; }

        void AddDomainEvent(DomainEvent @event);

        void RemoveDomainEvent(DomainEvent @event);

        void ClearDomainEvents();
    }
    public interface IBaseEntity : IBaseEntity<long> { }
}
