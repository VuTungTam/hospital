using Hospital.Application.Dtos.Employee;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Queries.Employees
{
    [RequiredPermission(ActionExponent.ViewEmployee)]
    public class GetEmployeesPaginationQuery : BaseQuery<PaginationResult<EmployeeDto>>
    {
        public GetEmployeesPaginationQuery(Pagination pagination, AccountStatus state, long zoneId, long roleId, long facilityId)
        {
            Pagination = pagination;
            State = state;
            RoleId = roleId;
            ZoneId = zoneId;
            FacilityId = facilityId;
        }

        public Pagination Pagination { get; }
        public AccountStatus State { get; }
        public long ZoneId { get; }
        public long RoleId { get; }
        public long FacilityId { get; }
    }
}
