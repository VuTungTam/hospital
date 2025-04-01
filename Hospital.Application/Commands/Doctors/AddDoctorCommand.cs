using Hospital.Application.Dtos.Doctors;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Doctors
{
    [RequiredPermission(ActionExponent.AddDoctor)]
    public class AddDoctorCommand : BaseCommand<string>
    {
        public AddDoctorCommand(DoctorDto doctor) 
        {
            Doctor = doctor;
        }

        public DoctorDto Doctor { get; }

    }
}
