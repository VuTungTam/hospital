using Hospital.Domain.Entities.Articles;
using Hospital.Domain.Enums;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Articles
{
    public class ArticleByStatusEqualsSpecification : ExpressionSpecification<Article>
    {
        public ArticleByStatusEqualsSpecification(ArticleStatus status) : base(x => status == ArticleStatus.None || x.Status == status)
        {
        }
    }
}
