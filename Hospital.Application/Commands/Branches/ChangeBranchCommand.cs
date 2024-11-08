using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Branches
{
    public class ChangeBranchCommand : BaseCommand<string>
    {
        public ChangeBranchCommand(long id)
        {
            Id = id;
        }

        public long Id { get; }
    }
}
