using Hospital.Application.Dtos.Articles;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Articles
{
    //[RequiredPermission(ActionExponent.ArticleManagement)]
    public class AddArticleCommand : BaseCommand<string>
    {
        public AddArticleCommand(ArticleDto article)
        {
            Article = article;
        }

        public ArticleDto Article { get; }
    }
}
