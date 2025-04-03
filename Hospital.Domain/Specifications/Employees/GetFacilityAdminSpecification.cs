using Hospital.Domain.Constants;
using Hospital.SharedKernel.Domain.Entities.Employees;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Employees
{
    public class GetFacilityAdminSpecification : ExpressionSpecification<Employee>
    {
        public GetFacilityAdminSpecification() : base(x => x.EmployeeRoles.Where(r => r.Role.Code == RoleCodeConstant.FACILITY_ADMIN).Any())
        {
            
        }
    }
}
