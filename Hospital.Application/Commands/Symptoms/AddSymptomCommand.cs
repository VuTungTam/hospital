using Hospital.Application.Dtos.Symptoms;
using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Symptoms
{
    [RequiredPermission(ActionExponent.SymptomManagement)]
    public class AddSymptomCommand : BaseCommand<string>
    {
        public AddSymptomCommand(SymptomDto symptom)
        {
            Symptom = symptom;
        }
        public SymptomDto Symptom { get; set; }
    }
}
