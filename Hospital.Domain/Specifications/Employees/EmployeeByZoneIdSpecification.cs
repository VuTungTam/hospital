using Hospital.SharedKernel.Domain.Entities.Employees;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Employees
{
    public class EmployeeByZoneIdSpecification : ExpressionSpecification<Employee>
    {
        public EmployeeByZoneIdSpecification(long zoneId) : base(x=> x.ZoneId == zoneId)
        {
        }
    }
}
