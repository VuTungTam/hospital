using Hospital.Application.Dtos.Doctors;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Doctors
{
    public class UpdateDoctorCommand : BaseCommand
    {
        public UpdateDoctorCommand(DoctorDto doctor)
        {
            Doctor = doctor;
        }

        public DoctorDto Doctor { get; }
    }
}
