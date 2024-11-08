using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Symptoms
{
    [RequiredPermission(ActionExponent.SymptomManagement)]
    public class DeleteSymptomCommand : BaseCommand
    {
        public DeleteSymptomCommand(List<long> ids)
        {
            Ids = ids;
        }

        public List<long> Ids { get; }
    }
}
