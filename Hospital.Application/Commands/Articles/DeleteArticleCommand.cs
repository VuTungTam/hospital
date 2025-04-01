using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Articles
{
    [RequiredPermission(ActionExponent.ArticleManagement)]
    public class DeleteArticleCommand : BaseCommand
    {
        public DeleteArticleCommand(List<long> ids)
        {
            Ids = ids;
        }

        public List<long> Ids { get; }
    }
}
