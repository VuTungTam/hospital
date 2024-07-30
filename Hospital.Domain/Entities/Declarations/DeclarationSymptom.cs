using Hospital.Domain.Entities.Symptoms;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Domain.Entities.Declarations
{
    [Table("DeclarationSymptom")]
    public class DeclarationSymptom :
        BaseEntity,
        ICreated,
        ICreator,
        IModified,
        IModifier,
        ISoftDelete,
        IDeletedBy
    {
        public long DeclarationId { get; set; }
        public Declaration Declaration { get; set; }
        public long SymptomId { get; set; }
        public Symptom Symptom { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public long? Creator { get; set; }
        public DateTime? Modified { get; set; }
        public long? Modifier { get; set; }
        public DateTime? Deleted { get; set; }
        public long? DeletedBy { get; set; }
    }
}
