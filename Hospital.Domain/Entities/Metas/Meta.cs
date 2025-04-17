using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.Domain.Entities.Metas
{
    [Table("tbl_metas")]
    public class Meta
     : BaseEntity,
       ICreatedAt,
       ICreatedBy,
       IModifiedAt,
       IModifiedBy
    {
        public string Name { get; set; }

        public string Title { get; set; }

        public string Content { get; set; }

        public string Module { get; set; }

        public string Page { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public long? CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public long? ModifiedBy { get; set; }
    }
}
