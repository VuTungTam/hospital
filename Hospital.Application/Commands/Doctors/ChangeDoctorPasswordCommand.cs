using Hospital.Application.Models.Auth;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Doctors
{
    [RequiredPermission(ActionExponent.Update)]
    public class ChangeDoctorPasswordCommand : BaseCommand
    {
        public ChangeDoctorPasswordCommand(ChangePasswordRequest dto)
        {
            Dto = dto;
        }

        public ChangePasswordRequest Dto { get; }
    }
}
