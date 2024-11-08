using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.SharedKernel.Application.Services.Auth.Entities
{
    [Table("perm_actions")]
    public class Action :
        BaseEntity,
        ISystemEntity
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Exponent { get; set; }

        public bool IsInternal { get; set; }

        public virtual List<RoleAction> RoleActions { get; set; }
    }
}
