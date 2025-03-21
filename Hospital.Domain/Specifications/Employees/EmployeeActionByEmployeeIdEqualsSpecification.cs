using Hospital.SharedKernel.Domain.Entities.Employees;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Employees
{
    public class EmployeeActionByEmployeeIdEqualsSpecification : ExpressionSpecification<EmployeeAction>
    {
        public EmployeeActionByEmployeeIdEqualsSpecification(long employeeId) : base(x => x.EmployeeId == employeeId)
        {
        }
    }
}
