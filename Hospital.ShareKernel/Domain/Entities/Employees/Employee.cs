using System.ComponentModel.DataAnnotations.Schema;
using Hospital.SharedKernel.Domain.Entities.Interfaces;
using Hospital.SharedKernel.Domain.Entities.Users;

namespace Hospital.SharedKernel.Domain.Entities.Employees
{
    [Table("mcs_employees")]
    public class Employee :
        BaseUser,
        IFacility,
        IZone
    {
        public List<EmployeeRole> EmployeeRoles { get; set; }

        public long FacilityId { get; set; }

        public long ZoneId { get; set; }
    }
}
