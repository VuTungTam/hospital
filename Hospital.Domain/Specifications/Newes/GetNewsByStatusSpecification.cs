using Hospital.Domain.Entities.Newses;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Newes
{
    public class GetNewsByStatusSpecification : ExpressionSpecification<News>
    {
        public GetNewsByStatusSpecification(NewsStatus status) : base(x => (status == NewsStatus.None || x.Status == status))
        {
        }
    }
}
