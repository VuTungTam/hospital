using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Domain.Enums;

namespace Hospital.Application.Commands.Doctors
{
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
