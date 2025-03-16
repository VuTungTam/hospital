using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Libraries.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.SharedKernel.Domain.Entities.Auths
{
    [Table("perm_actions")]
    public class Action :
        BaseEntity,
        ISystemEntity
    {
        public string Code { get; set; }

        [Filterable("Tên")]
        public string Name { get; set; }

        public string NameEn { get; set; }

        public string Description { get; set; }

        public int Exponent { get; set; }

        public bool IsInternal { get; set; }

        public long ParentId { get; set; }

        public virtual List<RoleAction> RoleActions { get; set; }
    }
}
