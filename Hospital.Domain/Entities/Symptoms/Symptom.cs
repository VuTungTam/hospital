using Hospital.Domain.Entities.Bookings;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Domain.Entities.Symptoms
{
    [Table("Symptoms")]
    public class Symptom : 
        BaseEntity,
        IModified,
        IModifier,
        ICreated,
        ICreator,
        ISoftDelete,
        IDeletedBy
    {
        public string NameVn { get; set; }
        public string NameEn { get; set; }
        public List<BookingSymptom> BookingSymptom { get; set; }
        public DateTime? Modified { get; set; }
        public long? Modifier { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public long? Creator { get; set; }
        public DateTime? Deleted { get; set; }
        public long? DeletedBy { get; set; }
    }
}
