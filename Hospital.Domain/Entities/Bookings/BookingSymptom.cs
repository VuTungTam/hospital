using Hospital.Domain.Entities.Symptoms;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Hospital.Domain.Entities.Bookings
{
    [Table("tbl_booking_symptom")]
    public class BookingSymptom :
        BaseEntity,
        ICreatedBy,
        ICreatedAt
    {
        public long BookingId { get; set; }
        [JsonIgnore]
        [IgnoreDataMember]
        public Booking Booking { get; set; }
        public long SymptomId { get; set; }
        [JsonIgnore]
        public Symptom Symptom { get; set; }

        public DateTime CreatedAt { get; set; }

        public long? CreatedBy { get; set; }

        
    }
}
