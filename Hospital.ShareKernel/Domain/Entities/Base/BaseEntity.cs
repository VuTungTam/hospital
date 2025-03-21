using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Domain.Events.BaseEvents;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Hospital.SharedKernel.Domain.Entities.Base
{
    public class BaseEntity : Entity, IBaseEntity
    {
        private List<DomainEvent> _domainEvents;
        [Key]

        [DatabaseGenerated(DatabaseGeneratedOption.None)]

        public long Id { get; set; }

        [NotMapped, JsonIgnore]
        public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents?.AsReadOnly();

        public void AddDomainEvent(DomainEvent @event)
        {
            _domainEvents = _domainEvents ?? new List<DomainEvent>();
            _domainEvents.Add(@event);
        }

        public void RemoveDomainEvent(DomainEvent @event)
        {
            _domainEvents?.Remove(@event);
        }

        public void ClearDomainEvents()
        {
            _domainEvents = null;
        }
    }
}
