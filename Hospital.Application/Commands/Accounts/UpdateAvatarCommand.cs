using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;
namespace Hospital.Application.Commands.Accounts
{
    [RequiredPermission(ActionExponent.Update)]
    public class UpdateAvatarCommand : BaseCommand
    {
        public UpdateAvatarCommand(string fileName)
        {
            FileName = fileName;
        }

        public string FileName { get; }
    }
}
