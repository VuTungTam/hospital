using Hospital.Application.Dtos.HealthProfiles;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.HealthProfiles
{
    public class GetHealthProfileByIdQuery : BaseQuery<HealthProfileDto>
    {
        public GetHealthProfileByIdQuery(long id) 
        {
            Id = id;
        }
        public long Id { get; set; }
    }
}
