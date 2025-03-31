using Hospital.Application.Dtos.Doctors;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.Doctors
{
    public class AddDoctorCommand : BaseCommand<string>
    {
        public AddDoctorCommand(DoctorDto doctor) 
        {
            Doctor = doctor;
        }

        public DoctorDto Doctor { get; }

    }
}
