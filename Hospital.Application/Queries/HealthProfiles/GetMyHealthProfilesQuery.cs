using Hospital.Application.Dtos.HealthProfiles;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Models.Requests;
using Hospital.SharedKernel.Application.Models.Responses;

namespace Hospital.Application.Queries.HealthProfiles
{
    public class GetMyHealthProfilesQuery : BaseQuery<List<HealthProfileDto>>
    {
        public GetMyHealthProfilesQuery()
        {
            
        }
    }
}
