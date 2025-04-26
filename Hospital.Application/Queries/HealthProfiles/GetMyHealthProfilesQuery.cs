using Hospital.Application.Dtos.HealthProfiles;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.HealthProfiles
{
    public class GetMyHealthProfilesQuery : BaseQuery<List<HealthProfileDto>>
    {
        public GetMyHealthProfilesQuery()
        {

        }
    }
}
