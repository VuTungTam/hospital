using Hospital.Domain.Entities.Bookings;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Domain.Entities.QueueItems
{
    [Table("QueueItems")]
    public class QueueItem : BaseEntity, ICreated, ICreator
    {
        public Booking Booking { get; set; }
        public long BookingId { get; set; }
        public int Position { get; set; }
        public int State { get; set; }
        public DateTime Created { get; set; }
        public long? Creator { get; set ; }
    }
}
