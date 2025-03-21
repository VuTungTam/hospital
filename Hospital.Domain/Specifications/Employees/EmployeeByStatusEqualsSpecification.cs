using Hospital.SharedKernel.Domain.Entities.Employees;
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Employees
{
    public class EmployeeByStatusEqualsSpecification : ExpressionSpecification<Employee>
    {
        public EmployeeByStatusEqualsSpecification(AccountStatus status) : base(x => x.Status == status)
        {
        }
    }
}
