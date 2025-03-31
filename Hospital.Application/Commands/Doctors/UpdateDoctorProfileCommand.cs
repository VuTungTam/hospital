using Hospital.Application.Models;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Doctors
{
    public class UpdateDoctorProfileCommand : BaseCommand
    {
        public UpdateDoctorProfileCommand(UpdateProfileModel model)
        {
            Model = model;
        }

        public UpdateProfileModel Model { get; }
    }
}
