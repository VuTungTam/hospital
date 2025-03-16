using Hospital.SharedKernel.Application.CQRS.Queries.Base;

namespace Hospital.Application.Queries.Articles
{
    public class GetArticleViewCountQuery : BaseAllowAnonymousQuery<int>
    {
        public GetArticleViewCountQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }
}
