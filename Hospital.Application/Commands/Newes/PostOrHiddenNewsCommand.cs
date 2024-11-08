using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Newes
{
    [RequiredPermission(ActionExponent.UIManagement)]
    public class PostOrHiddenNewsCommand : BaseCommand
    {
        public PostOrHiddenNewsCommand(long id, bool isPost)
        {
            Id = id;
            IsPost = isPost;
        }

        public long Id { get; }
        public bool IsPost { get; }
    }
}
