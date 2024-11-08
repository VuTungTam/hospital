using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Newes
{
    [RequiredPermission(ActionExponent.UIManagement)]
    public class DeleteNewsCommand : BaseCommand
    {
        public DeleteNewsCommand(List<long> ids)
        {
            Ids = ids;
        }

        public List<long> Ids { get; }
    }
}
