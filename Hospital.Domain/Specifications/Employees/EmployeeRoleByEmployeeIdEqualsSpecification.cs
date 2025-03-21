using Hospital.SharedKernel.Domain.Entities.Employees;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Employees
{
    public class EmployeeRoleByEmployeeIdEqualsSpecification : ExpressionSpecification<EmployeeRole>
    {
        public EmployeeRoleByEmployeeIdEqualsSpecification(long employeeId) : base(x => x.EmployeeId == employeeId)
        {
        }
    }
}
