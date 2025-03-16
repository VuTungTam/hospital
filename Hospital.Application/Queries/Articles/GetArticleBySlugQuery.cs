using Hospital.SharedKernel.Application.CQRS.Queries.Base;
using Hospital.Application.Dtos.Articles;

namespace Hospital.Application.Queries.Articles
{
    public class GetArticleBySlugQuery : BaseAllowAnonymousQuery<ArticleDto>
    {
        public GetArticleBySlugQuery(string slug, List<string> langs)
        {
            Slug = slug;
            Langs = langs;
        }

        public string Slug { get; }
        public List<string> Langs { get; }
    }
}
