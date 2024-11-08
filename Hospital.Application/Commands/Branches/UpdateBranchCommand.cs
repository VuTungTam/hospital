using Hospital.Application.Dtos.Branches;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Branches
{
    [RequiredPermission(ActionExponent.Master)]
    public class UpdateBranchCommand : BaseCommand
    {
        public UpdateBranchCommand(BranchDto branch)
        {
            Branch = branch;
        }

        public BranchDto Branch { get; }
    }
}
