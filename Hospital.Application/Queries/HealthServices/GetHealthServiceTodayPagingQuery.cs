using Hospital.Application.Dtos.HealthServices;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;

namespace Hospital.Application.Queries.HealthServices
{
    public class GetHealthServiceTodayPagingQuery : BaseAllowAnonymousQuery<PaginationResult<HealthServiceDto>>
    {
        public GetHealthServiceTodayPagingQuery(Pagination pagination, long facilityId, long typeId, long specialtyId, long doctorId, HealthServiceStatus status)
        {
            Pagination = pagination;
            FacilityId = facilityId;
            TypeId = typeId;
            SpecialtyId = specialtyId;
            DoctorId = doctorId;
            Status = status;
        }
        public Pagination Pagination { get; }
        public long FacilityId { get; }
        public long TypeId { get; }
        public long SpecialtyId { get; }
        public long DoctorId { get; }
        public HealthServiceStatus Status { get; }
    }
}
