using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Doctors
{
    [RequiredPermission(ActionExponent.DeleteDoctor)]
    public class DeleteDoctorsCommand : BaseCommand
    {
        public DeleteDoctorsCommand(List<long> ids)
        {
            Ids = ids;
        }

        public List<long> Ids { get; }
    }
}
