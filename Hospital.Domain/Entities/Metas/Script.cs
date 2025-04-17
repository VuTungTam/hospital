using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Domain.Entities.Metas
{
    [Table("tbl_scripts")]
    public class Script
      : BaseEntity,
        ICreatedAt,
        ICreatedBy,
        IModifiedAt,
        IModifiedBy
    {
        public string Header { get; set; }

        public string Body { get; set; }

        public string Footer { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public long? CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public long? ModifiedBy { get; set; }
    }
}
