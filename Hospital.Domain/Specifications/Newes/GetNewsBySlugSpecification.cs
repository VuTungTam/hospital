using Hospital.Domain.Entities.Newses;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Newes
{
    public class GetNewsBySlugSpecification : ExpressionSpecification<News>
    {
        public GetNewsBySlugSpecification(string slug) : base(x => x.Slug == slug)
        {
        }
    }
}
