using Hospital.Application.Dtos.HealthFacility;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;

namespace Hospital.Application.Queries.HealthFacilities
{
    public class GetHealthFacilityPaginationQuery : BaseAllowAnonymousQuery<PaginationResult<HealthFacilityDto>>
    {
        public GetHealthFacilityPaginationQuery(Pagination pagination, long typeId, long serviceTypeId, int pid, HealthFacilityStatus status)
        {
            Pagination = pagination;
            TypeId = typeId;
            Status = status;
            ServiceTypeId = serviceTypeId;
            Pid = pid;
        }
        public Pagination Pagination { get; }
        public long TypeId { get; }
        public long ServiceTypeId { get; }
        public int Pid { get; }
        public HealthFacilityStatus Status { get; }
    }
}
