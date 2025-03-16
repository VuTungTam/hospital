using Hospital.SharedKernel.Domain.Entities.Users;
using System.ComponentModel.DataAnnotations.Schema;

namespace Hospital.SharedKernel.Domain.Entities.Employees
{
    [Table("mcs_employees")]
    public class Employee : BaseUser
    {
        public string ScheduleColor { get; set; }

        public List<EmployeeRole> EmployeeRoles { get; set; }

        public List<EmployeeAction> EmployeeActions { get; set; }

    }
}
