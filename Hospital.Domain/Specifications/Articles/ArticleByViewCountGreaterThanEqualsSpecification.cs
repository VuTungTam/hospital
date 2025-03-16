using Hospital.Domain.Entities.Articles;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Articles
{
    public class ArticleByViewCountGreaterThanEqualsSpecification : ExpressionSpecification<Article>
    {
        public ArticleByViewCountGreaterThanEqualsSpecification(int value) : base(x => x.ViewCount > value)
        {
        }
    }
}
