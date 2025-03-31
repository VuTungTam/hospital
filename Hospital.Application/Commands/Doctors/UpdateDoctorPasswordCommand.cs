using Hospital.Application.Models;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Doctors
{
    public class UpdateDoctorPasswordCommand : BaseCommand
    {
        public UpdateDoctorPasswordCommand(UserPwdModel model)
        {
            Model = model;
        }

        public UserPwdModel Model { get; }
    }
}
