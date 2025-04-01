using Hospital.Application.Dtos.Articles;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Articles
{
    [RequiredPermission(ActionExponent.ArticleManagement)]
    public class AddArticleCommand : BaseCommand<string>
    {
        public AddArticleCommand(ArticleDto article)
        {
            Article = article;
        }

        public ArticleDto Article { get; }
    }
}
