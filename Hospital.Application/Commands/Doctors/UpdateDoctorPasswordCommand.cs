using Hospital.Application.Models;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Doctors
{
    [RequiredPermission(ActionExponent.UpdateDoctor)]
    public class UpdateDoctorPasswordCommand : BaseCommand
    {
        public UpdateDoctorPasswordCommand(UserPwdModel model)
        {
            Model = model;
        }

        public UserPwdModel Model { get; }
    }
}
