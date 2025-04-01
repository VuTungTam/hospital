using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Domain.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Doctors
{
    [RequiredPermission(ActionExponent.UpdateDoctor)]
    public class UpdateDoctorStatusCommand : BaseCommand
    {
        public UpdateDoctorStatusCommand(long id, AccountStatus status)
        {
            Id = id;
            Status = status;
        }

        public long Id { get; }
        public AccountStatus Status { get; }
    }
}
