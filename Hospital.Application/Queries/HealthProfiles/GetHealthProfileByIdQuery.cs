using Hospital.Application.Dtos.HealthProfiles;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

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
