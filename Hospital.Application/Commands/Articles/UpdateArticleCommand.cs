using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Libraries.Attributes;
using Hospital.Application.Dtos.Articles;
using Hospital.SharedKernel.Application.Services.Auth.Enums;

namespace Hospital.Application.Commands.Articles
{
    [RequiredPermission(ActionExponent.ArticleManagement)]
    public class UpdateArticleCommand : BaseCommand
    {
        public UpdateArticleCommand(ArticleDto article)
        {
            Article = article;
        }

        public ArticleDto Article { get; }
    }
}
