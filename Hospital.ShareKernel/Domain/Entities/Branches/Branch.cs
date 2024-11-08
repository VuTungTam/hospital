using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Libraries.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.SharedKernel.Domain.Entities.Branches
{
    [Table("Branches")]
    public class Branch
       : BaseEntity,
         ICreated,
         IModified,
         IModifier,
         ISoftDelete,
         IDeletedBy
    {
        [Filterable("Tên")]
        public string Name { get; set; }

        [Filterable("Số điện thoại")]
        public string Phone { get; set; }

        [Filterable("Email")]
        public string Email { get; set; }

        [Filterable("Địa chỉ")]
        public string Address { get; set; }

        public DateTime? FoundingDate { get; set; }

        public bool? Active { get; set; }

        public DateTime Created { get; set; } = DateTime.Now;

        public DateTime? Modified { get; set; }

        public long? Modifier { get; set; }

        public DateTime? Deleted { get; set; }

        public long? DeletedBy { get; set; }
    }
}
