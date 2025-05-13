using System.ComponentModel.DataAnnotations.Schema;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Domain.Entities.Interfaces;

namespace Hospital.Domain.Entities.CancelReasons
{
    [Table("mcs_cancel_reasons")]
    public class CancelReason :
        ICreatedBy
    {
        public string Reason { get; set; }

        public long BookingId { get; set; }

        public CancelType CancelType { get; set; }

        public long? CreatedBy { get; set; }
    }
}
