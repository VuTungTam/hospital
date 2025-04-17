using Hospital.Application.Dtos.HealthFacility;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.HealthFacilities
{
    public class GetFacilityBySlugQuery : BaseAllowAnonymousQuery<HealthFacilityDto>
    {
        public GetFacilityBySlugQuery(string slug, List<string> langs)
        {
            Slug = slug;
            Langs = langs;
        }

        public string Slug { get; }

        public List<string> Langs { get; }
    }
}
