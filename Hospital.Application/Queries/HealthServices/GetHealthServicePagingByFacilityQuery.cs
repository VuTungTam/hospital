using Hospital.Application.Dtos.HealthServices;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;
using System;

namespace Hospital.Application.Queries.HealthServices
{
    public class GetHealthServicePagingByFacilityQuery : BaseQuery<PagingResult<HealthServiceDto>>
    {
        public GetHealthServicePagingByFacilityQuery(Pagination pagination, long facilityId)
        {
            Pagination = pagination;
            FacilityId = facilityId;
        }
        public Pagination Pagination { get; }
        public long FacilityId { get; }
    }
    
}
