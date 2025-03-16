using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Domain.Entities.Feedbacks
{
    [Table("tbl_feedbacks")]
    public class Feedback :
        BaseEntity,
        ICreatedAt,
        ICreatedBy
    {
        public long ReferId { get; set; }

        public int Stars { get; set; }

        public string Message { get; set; }

        public DateTime CreatedAt { get; set; }

        public long? CreatedBy { get; set; }
    }
}
