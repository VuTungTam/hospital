using Hospital.SharedKernel.Domain.Entities.Employees;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Employees
{
    public class EmployeeByEmailEqualsSpecification : ExpressionSpecification<Employee>
    {
        public EmployeeByEmailEqualsSpecification(string email) : base(x => x.Email == email)
        {
        }
    }
}
