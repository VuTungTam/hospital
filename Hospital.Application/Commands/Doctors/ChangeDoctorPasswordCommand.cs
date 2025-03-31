using Hospital.Application.Models.Auth;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Doctors
{
    public class ChangeDoctorPasswordCommand : BaseCommand
    {
        public ChangeDoctorPasswordCommand(ChangePasswordRequest dto)
        {
            Dto = dto;
        }

        public ChangePasswordRequest Dto { get; }
    }
}
