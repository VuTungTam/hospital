using Hospital.SharedKernel.Domain.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.SharedKernel.Application.Services.Auth.Entities
{
    [Table("perm_roles_actions")]
    public class RoleAction : BaseEntity
    {
        public long RoleId { get; set; }

        public long ActionId { get; set; }

        public Role Role { get; set; }

        public Action Action { get; set; }
    }
}
