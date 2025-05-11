using Hospital.Application.Dtos.HealthServices;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.HealthServices
{
    public class GetServiceTypeQuery : BaseAllowAnonymousQuery<List<ServiceTypeDto>>
    {
        public GetServiceTypeQuery()
        {
        }
    }
}
