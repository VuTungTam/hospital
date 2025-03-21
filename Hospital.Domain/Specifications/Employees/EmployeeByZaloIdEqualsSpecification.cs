using Hospital.SharedKernel.Domain.Entities.Employees;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Employees
{
    public class EmployeeByZaloIdEqualsSpecification : ExpressionSpecification<Employee>
    {
        public EmployeeByZaloIdEqualsSpecification(string zaloId) : base(x => x.ZaloId == zaloId)
        {
        }
    }
}
