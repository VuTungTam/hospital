using Hospital.SharedKernel.Domain.Entities.Base;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.SharedKernel.Domain.Entities.Employees
{
    [Table("perm_employee_action_map")]
    public class EmployeeAction :
        BaseEntity,
        ICreatedAt,
        ICreatedBy
    {
        public long EmployeeId { get; set; }

        public long ActionId { get; set; }

        public bool IsExclude { get; set; }

        public Auths.Action Action { get; set; }

        public DateTime CreatedAt { get; set; }

        public long? CreatedBy { get; set; }
    }
}
