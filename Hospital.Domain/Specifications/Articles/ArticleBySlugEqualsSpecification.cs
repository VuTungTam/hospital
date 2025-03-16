using Hospital.Domain.Entities.Articles;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Articles
{
    public class ArticleBySlugEqualsSpecification : ExpressionSpecification<Article>
    {
        public ArticleBySlugEqualsSpecification(string slug) : base(x => x.Slug == slug)
        {
        }
    }
}
