using Hospital.SharedKernel.Domain.Entities.Employees;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Employees
{
    public class EmployeeByFacilityIdEqualsSpecification : ExpressionSpecification<Employee>
    {
        public EmployeeByFacilityIdEqualsSpecification(long facilityId) : base(x => x.FacilityId == facilityId)
        {
        }
    }
}
