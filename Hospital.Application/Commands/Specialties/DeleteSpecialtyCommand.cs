using Hospital.SharedKernel.Application.CQRS.Commands.Base;
using Hospital.SharedKernel.Application.Services.Auth.Enums;
using Hospital.SharedKernel.Libraries.Attributes;

namespace Hospital.Application.Commands.Specialties
{
    [RequiredPermission(ActionExponent.Master)]
    public class DeleteSpecialtyCommand : BaseCommand
    {
        public DeleteSpecialtyCommand(List<long> ids) 
        {
            Ids = ids;
        }
        public List<long> Ids { get; set; }
    }
}
