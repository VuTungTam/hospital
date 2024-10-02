using Hospital.Domain.Entities.Declarations;
using Hospital.Domain.Entities.HealthServices;
using Hospital.Domain.Entities.QueueItems;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Domain.Entities.Visits
{
    [Table("Visits")]
    public class Visit : BaseEntity,
        ICreated,
        ICreator
    {
        public long DeclarationId { get; set; }
        public Declaration Declaration { get; set; }
        public long ServiceId { get; set; }
        public HealthService Service { get; set; }
        public List<QueueItem> QueueItems { get; set; }
        public List<VisitSymptom> VisitSymptom { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public long? Creator { get; set; }
    }
}
