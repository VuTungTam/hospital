using Hospital.SharedKernel.Domain.Entities.Auths;
using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.SharedKernel.Domain.Entities.Employees
{
    [Table("perm_employee_role_map")]
    public class EmployeeRole :
        BaseEntity,
        ICreatedAt,
        ICreatedBy,
        IModifiedAt,
        IModifiedBy,
        ISoftDelete,
        IDeletedBy
    {
        public long RoleId { get; set; }

        public long EmployeeId { get; set; }

        public Role Role { get; set; }

        public Employee Employee { get; set; }

        public DateTime CreatedAt { get; set; }

        public long? CreatedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public long? ModifiedBy { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedAt { get; set; }

        public long? DeletedBy { get; set; }
    }
}
