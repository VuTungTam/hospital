using Hospital.SharedKernel.Domain.Entities.Employees;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Employees
{
    public class EmployeeByCodeEqualsSpecification : ExpressionSpecification<Employee>
    {
        public EmployeeByCodeEqualsSpecification(string code) : base(x => x.Code == code)
        {
        }
    }
}
