using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Articles
{
    public class UpdateArticleViewCountCommand : BaseAllowAnonymousCommand
    {
        public UpdateArticleViewCountCommand(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }
}
