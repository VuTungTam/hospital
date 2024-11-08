using Hospital.SharedKernel.Application.CQRS.Commands.Base;

namespace Hospital.Application.Commands.HealthServices
{
    public class DeleteHealthServiceCommand : BaseCommand
    {
        public DeleteHealthServiceCommand(List<long> ids) 
        {
            Ids = ids;
        }
        public List<long> Ids { get; }
    }
}
