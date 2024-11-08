using Hospital.Domain.Entities.Newses;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Newes
{
    public class GetNewsByPostDateSpecification : ExpressionSpecification<News>
    {
        public GetNewsByPostDateSpecification(DateTime postDate) : base(x => (postDate == default || x.PostDate == postDate))
        {
        }
    }
}
