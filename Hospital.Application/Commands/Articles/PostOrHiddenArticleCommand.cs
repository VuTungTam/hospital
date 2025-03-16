using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Articles
{
    //[RequiredPermission(ActionExponent.ArticleManagement)]
    public class PostOrHiddenArticleCommand : BaseCommand
    {
        public PostOrHiddenArticleCommand(long id, bool isPost)
        {
            Id = id;
            IsPost = isPost;
        }

        public long Id { get; }
        public bool IsPost { get; }
    }
}
