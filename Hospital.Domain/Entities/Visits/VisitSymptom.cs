using Hospital.Domain.Entities.Symptoms;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;

namespace Hospital.Domain.Entities.Visits
{
    public class VisitSymptom :
        BaseEntity,
        ICreator,
        ICreated
    {
        public long VisitId { get; set; }
        public Visit Visit { get; set; }
        public long SymptomId { get; set; }
        public Symptom Symptom { get; set; }
        public long? Creator { get ; set ; }
        public DateTime Created { get ; set ; }
    }
}
