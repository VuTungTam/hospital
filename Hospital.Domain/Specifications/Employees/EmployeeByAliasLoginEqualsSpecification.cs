using Hospital.SharedKernel.Domain.Entities.Employees;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Employees
{
    public class EmployeeByAliasLoginEqualsSpecification : ExpressionSpecification<Employee>
    {
        public EmployeeByAliasLoginEqualsSpecification(string alias) : base(x => x.AliasLogin == alias)
        {
        }
    }
}
