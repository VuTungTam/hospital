using Hospital.Application.Dtos.Specialties;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Specialties
{
    [RequiredPermission(ActionExponent.SpecialtyManagement)]
    public class AddSpecialtyCommand : BaseCommand<string>
    {
        public AddSpecialtyCommand(SpecialtyDto specialty)
        {
            Specialty = specialty;
        }
        public SpecialtyDto Specialty { get; set; }
    }
}
