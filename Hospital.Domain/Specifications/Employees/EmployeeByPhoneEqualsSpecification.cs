using Hospital.SharedKernel.Domain.Entities.Employees;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Employees
{
    public class EmployeeByPhoneEqualsSpecification : ExpressionSpecification<Employee>
    {
        public EmployeeByPhoneEqualsSpecification(string phone) : base(x => x.Phone == phone)
        {
        }
    }
}
