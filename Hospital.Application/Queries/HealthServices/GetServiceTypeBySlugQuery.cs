using Hospital.Application.Dtos.HealthServices;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.HealthServices
{
    public class GetServiceTypeBySlugQuery : BaseAllowAnonymousQuery<ServiceTypeDto>
    {
        public GetServiceTypeBySlugQuery(string slug, List<string> langs)
        {
            Slug = slug;
            Langs = langs;
        }

        public string Slug { get; }

        public List<string> Langs { get; }
    }
}
