using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.SharedKernel.Application.Services.Auth.Entities
{
    [Table("perm_roles")]
    public class Role : BaseEntity, ISystemEntity
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public List<RoleAction> RoleActions { get; set; }

        public List<UserRole> UserRoles { get; set; }
    }
}
