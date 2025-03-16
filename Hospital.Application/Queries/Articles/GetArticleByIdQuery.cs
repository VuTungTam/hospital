using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.Application.Dtos.Articles;

namespace Hospital.Application.Queries.Articles
{
    public class GetArticleByIdQuery : BaseAllowAnonymousQuery<ArticleDto>
    {
        public GetArticleByIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }
}
