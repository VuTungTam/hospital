using Hospital.Domain.Entities.Symptoms;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Hospital.Domain.Entities.Bookings
{
    [Table("BookingSymptom")]
    public class BookingSymptom :
        BaseEntity,
        ICreator,
        ICreated
    {
        public long BookingId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public Booking Booking { get; set; }
        public long SymptomId { get; set; }
        [JsonIgnore]
        public Symptom Symptom { get; set; }
        public long? Creator { get ; set ; }
        public DateTime Created { get ; set ; }
    }
}
