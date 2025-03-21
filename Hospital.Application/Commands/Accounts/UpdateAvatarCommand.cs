using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Accounts
{
    public class UpdateAvatarCommand : BaseCommand
    {
        public UpdateAvatarCommand(string fileName)
        {
            FileName = fileName;
        }

        public string FileName { get; }
    }
}
