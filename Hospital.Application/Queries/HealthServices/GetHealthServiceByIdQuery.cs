using Hospital.Application.Dtos.HealthServices;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.HealthServices
{
    public class GetHealthServiceByIdQuery : BaseAllowAnonymousQuery<HealthServiceDto>
    {
        public GetHealthServiceByIdQuery(long id)
        {
            Id = id;
        }
        public long Id { get; set; }
    }
}
