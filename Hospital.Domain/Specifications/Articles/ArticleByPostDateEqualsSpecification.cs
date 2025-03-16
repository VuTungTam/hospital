using Hospital.Domain.Entities.Articles;
using Hospital.SharedKernel.Specifications;

namespace Hospital.Domain.Specifications.Articles
{
    public class ArticleByPostDateEqualsSpecification : ExpressionSpecification<Article>
    {
        public ArticleByPostDateEqualsSpecification(DateTime postDate) : base(x => postDate == default || x.PostDate == postDate)
        {
        }
    }
}
