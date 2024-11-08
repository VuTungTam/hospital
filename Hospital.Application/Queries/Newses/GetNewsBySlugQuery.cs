using Hospital.Application.Dtos.Newes;
using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.Newses
{
    public class GetNewsBySlugQuery : BaseAllowAnonymousQuery<NewsDto>
    {
        public GetNewsBySlugQuery(string slug)
        {
            Slug = slug;
        }

        public string Slug { get; }
    }
}
