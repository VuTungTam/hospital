using Hospital.Application.Dtos.HealthServices;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;

namespace Hospital.Application.Queries.HealthServices
{
    public class GetHealthServiceCurrentQuery : BaseAllowAnonymousQuery<PaginationResult<HealthServiceDto>>
    {
        public GetHealthServiceCurrentQuery(Pagination pagination, long facilityId, long doctorId)
        {
            DoctorId = doctorId;
            FacilityId = facilityId;
            Pagination = pagination;
        }
        public long DoctorId { get; set; }

        public long FacilityId { get; set; }

        public Pagination Pagination { get; }
    }
}
