using Hospital.Application.Dtos.Doctors;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Doctors
{
    [RequiredPermission(ActionExponent.UpdateDoctor)]
    public class UpdateDoctorCommand : BaseCommand
    {
        public UpdateDoctorCommand(DoctorDto doctor)
        {
            Doctor = doctor;
        }

        public DoctorDto Doctor { get; }
    }
}
