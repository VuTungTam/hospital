using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Employees;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Libraries.Attributes;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.SharedKernel.Domain.Entities.Auths
{
    [Table("perm_roles")]
    public class Role : BaseEntity, ISystemEntity
    {
        public string Code { get; set; }

        [Filterable("Tên vai trò")]
        public string Name { get; set; }

        public string NameEn { get; set; }

        public List<RoleAction> RoleActions { get; set; }

        public List<EmployeeRole> UserRoles { get; set; }
    }
}
